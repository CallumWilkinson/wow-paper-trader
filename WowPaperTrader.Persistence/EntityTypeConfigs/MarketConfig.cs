using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

namespace WowPaperTrader.Persistence.EntityTypeConfigs;

public sealed class MarketConfig : IEntityTypeConfiguration<Market>
{
    public void Configure(EntityTypeBuilder<Market> builder)
    {
            // Keep the existing schema names so the C# rename does not require a database migration.
            builder.Property(market => market.MarketType)
                .HasConversion<int>()
                .HasColumnName("AuctionMarketType");

            builder.ToTable("AuctionMarkets", table =>
            {
                table.HasCheckConstraint(
                    "CK_AuctionMarkets_AuctionMarketType_ConnectedRealmId",
                    """
                    (
                        ("AuctionMarketType" = 1 AND "ConnectedRealmId" IS NULL)
                        OR
                        ("AuctionMarketType" = 2 AND "ConnectedRealmId" IS NOT NULL)
                    )
                    """
                );
            });

            builder
                .HasIndex(market => market.Region)
                .IsUnique()
                .HasDatabaseName("UX_AuctionMarkets_Region_RegionalCommodities")
                .HasFilter("\"AuctionMarketType\" = 1");
            
            builder.HasIndex(market => new { market.ConnectedRealmId, market.Region })
                .IsUnique()
                .HasDatabaseName(
                    "UX_AuctionMarkets_Region_ConnectedRealmId")
                .HasFilter("\"AuctionMarketType\" = 2");

            builder.HasData(new
            {
                Id = 1L,
                Region = "US",
                MarketType = MarketType.Commodity,
                ConnectedRealmId = (long?)null,
                DisplayName = "US Commodities"
            });
    }
}
