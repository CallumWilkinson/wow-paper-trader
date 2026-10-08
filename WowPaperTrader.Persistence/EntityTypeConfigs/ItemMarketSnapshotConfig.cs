using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

namespace WowPaperTrader.Persistence.EntityTypeConfigs;

public sealed class ItemMarketSnapshotConfig: IEntityTypeConfiguration<ItemMarketSnapshot>
{
    public void Configure(EntityTypeBuilder<ItemMarketSnapshot> builder)
    {
        builder.Property(snapshot => snapshot.MarketId).HasColumnName("AuctionMarketId");

        builder.HasKey(snapshot => new
        {
            snapshot.MarketId,
            snapshot.ItemId,
            snapshot.VariantKey,
            snapshot.ObservedAtUtc
        });

        builder.HasIndex(snapshot => snapshot.ObservedAtUtc);

        builder
            .HasOne(snapshot => snapshot.MarketSnapshot)
            .WithMany(snapshot => snapshot.ItemMarketSnapshots)
            .HasForeignKey(snapshot => new
            {
                snapshot.MarketId,
                snapshot.ObservedAtUtc
            })
            .HasConstraintName("FK_ItemMarketSnapshots_AuctionMarketSnapshots_AuctionMarketId_~");

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
