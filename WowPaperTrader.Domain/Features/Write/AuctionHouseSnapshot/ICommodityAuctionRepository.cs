using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot.WowApiResult;

namespace WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;

public interface ICommodityAuctionRepository
{
    Task<IngestionRun> CreateIngestionRunAsync(CancellationToken cancellationToken);

    Task SaveSnapshotAsync(
        IngestionRun run,
        WowApiResult<AuctionSnapshot> wowApiResult,
        CancellationToken cancellationToken);
}