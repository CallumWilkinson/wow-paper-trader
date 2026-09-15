using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

namespace WowPaperTrader.Persistence.EntityTypeConfigs;

public sealed class AuctionMarketConfig : IEntityTypeConfiguration<AuctionMarket>
{
    public void Configure(EntityTypeBuilder<AuctionMarket> builder)
    {
            builder.Property(auctionMarket => auctionMarket.AuctionMarketType).HasConversion<int>();

            builder.ToTable(table =>
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
                .HasIndex(auctionMarket => auctionMarket.Region)
                .IsUnique()
                .HasDatabaseName("UX_AuctionMarkets_Region_RegionalCommodities")
                .HasFilter("\"AuctionMarketType\" = 1");
            
            builder.HasIndex(auctionMarket => new { auctionMarket.ConnectedRealmId, auctionMarket.Region })
                .IsUnique()
                .HasDatabaseName(
                    "UX_AuctionMarkets_Region_ConnectedRealmId")
                .HasFilter("\"AuctionMarketType\" = 2");

            builder.HasData(new
            {
                Id = 1L,
                Region = "US",
                AuctionMarketType = AuctionMarketType.Commodity,
                ConnectedRealmId = (long?)null,
                DisplayName = "US Commodities"
            });
    }
}