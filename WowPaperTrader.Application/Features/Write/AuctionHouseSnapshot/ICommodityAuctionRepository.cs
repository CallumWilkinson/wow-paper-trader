using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.WowApiResult;

namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot;

public interface ICommodityAuctionRepository
{
    Task<IngestionRun> CreateIngestionRunAsync(CancellationToken cancellationToken);

    Task SaveSnapshotAsync(
        IngestionRun run,
        WowApiResult<AuctionSnapshot> wowApiResult,
        CancellationToken cancellationToken);
}