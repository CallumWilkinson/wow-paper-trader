using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

namespace WowPaperTrader.Persistence.EntityTypeConfigs;

public sealed class ItemMarketSnapshotConfig: IEntityTypeConfiguration<ItemMarketSnapshot>
{
    public void Configure(EntityTypeBuilder<ItemMarketSnapshot> builder)
    {
        builder.HasKey(snapshot => new
        {
            snapshot.AuctionMarketId,
            snapshot.ItemId,
            snapshot.VariantKey,
            snapshot.ObservedAtUtc
        });

        builder.HasIndex(snapshot => snapshot.ObservedAtUtc);

        builder
            .HasOne(snapshot => snapshot.AuctionMarketSnapshot)
            .WithMany(snapshot => snapshot.ItemMarketSnapshots)
            .HasForeignKey(snapshot => new
            {
                snapshot.AuctionMarketId,
                snapshot.ObservedAtUtc
            });

        builder
            .Property(snapshot => snapshot.LowerFenceUnitPrice)
            .HasPrecision(28, 6);
            
        builder
            .Property(snapshot => snapshot.UpperFenceUnitPrice)
            .HasPrecision(28, 6);

        builder
            .Property(snapshot => snapshot.FilteredMeanUnitPrice)
            .HasPrecision(28, 6);

        builder
            .Property(snapshot => snapshot.FilteredQuantityWeightedMeanUnitPrice)
            .HasPrecision(28, 6);
    }
}