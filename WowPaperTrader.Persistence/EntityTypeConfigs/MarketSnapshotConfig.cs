using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

namespace WowPaperTrader.Persistence.EntityTypeConfigs;

public sealed class MarketSnapshotConfig: IEntityTypeConfiguration<MarketSnapshot>
{
    public void Configure(EntityTypeBuilder<MarketSnapshot> builder)
    {
        builder.ToTable("AuctionMarketSnapshots");
        builder.Property(snapshot => snapshot.MarketId).HasColumnName("AuctionMarketId");

        builder.HasKey(snapshot => new
        {
            snapshot.MarketId,
            snapshot.ObservedAtUtc
        });

        builder
            .HasOne(snapshot => snapshot.Market)
            .WithMany(market => market.MarketSnapshots)
            .HasForeignKey(snapshot => snapshot.MarketId)
            .HasConstraintName("FK_AuctionMarketSnapshots_AuctionMarkets_AuctionMarketId");

        builder
            .HasOne(snapshot => snapshot.IngestionRun)
            .WithMany(run => run.MarketSnapshots)
            .HasForeignKey(snapshot => snapshot.IngestionRunId);
    }
}
