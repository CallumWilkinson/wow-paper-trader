using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

namespace WowPaperTrader.Persistence.EntityTypeConfigs;

public sealed class AuctionMarketSnapshotConfig: IEntityTypeConfiguration<AuctionMarketSnapshot>
{
    public void Configure(EntityTypeBuilder<AuctionMarketSnapshot> builder)
    {
        builder.HasKey(snapshot => new
        {
            snapshot.AuctionMarketId,
            snapshot.ObservedAtUtc
        });

        builder
            .HasOne(snapshot => snapshot.AuctionMarket)
            .WithMany(market => market.MarketSnapshots)
            .HasForeignKey(snapshot => snapshot.AuctionMarketId);

        builder
            .HasOne(snapshot => snapshot.IngestionRun)
            .WithMany(run => run.MarketSnapshots)
            .HasForeignKey(snapshot => snapshot.IngestionRunId);
    }
}