using WowPaperTrader.Domain.Contracts;
using WowPaperTrader.Domain.Entities;

namespace WowPaperTrader.Domain.Interfaces;

public interface ICommodityAuctionRepository
{
    Task<IngestionRun> CreateIngestionRunAsync(CancellationToken cancellationToken);

    Task SaveSnapshotAsync(
        IngestionRun run,
        WowApiResult<AuctionSnapshot> wowApiResult,
        CancellationToken cancellationToken);
}