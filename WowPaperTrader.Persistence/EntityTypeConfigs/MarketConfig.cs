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
                .HasColumnName("MarketType");

            builder.ToTable("Markets", table =>
            {
                table.HasCheckConstraint(
                    "CK_Markets_MarketType_ConnectedRealmId",
                    """
                    (
                        ("MarketType" = 1 AND "ConnectedRealmId" IS NULL)
                        OR
                        ("MarketType" = 2 AND "ConnectedRealmId" IS NOT NULL)
                    )
                    """
                );
            });

            builder
                .HasIndex(market => market.Region)
                .IsUnique()
                .HasDatabaseName("UX_Markets_Region_RegionalCommodities")
                .HasFilter("\"MarketType\" = 1");
            
            builder.HasIndex(market => new { market.ConnectedRealmId, market.Region })
                .IsUnique()
                .HasDatabaseName(
                    "UX_Markets_Region_ConnectedRealmId")
                .HasFilter("\"MarketType\" = 2");

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
