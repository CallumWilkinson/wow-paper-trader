using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

namespace WowPaperTrader.Persistence.EntityTypeConfigs;

public sealed class CurrentPriceLevelConfig: IEntityTypeConfiguration<CurrentPriceLevel>
{
    public void Configure(EntityTypeBuilder<CurrentPriceLevel> builder)
    {
        builder.Property(priceLevel => priceLevel.MarketId).HasColumnName("MarketId");

        builder.HasKey(priceLevel => new
        {
            priceLevel.MarketId,
            priceLevel.ItemId,
            priceLevel.VariantKey,
            priceLevel.UnitPrice
        });

        builder.HasOne(priceLevel => priceLevel.MarketSnapshot)
            .WithMany(marketSnapshot => marketSnapshot.CurrentPriceLevels)
            .HasForeignKey(priceLevel => new
            {
                priceLevel.MarketId,
                priceLevel.ObservedAtUtc
            })
            .HasConstraintName("FK_CurrentPriceLevels_MarketSnapshots_MarketId_O~");
    }
}
