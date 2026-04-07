using WowPaperTrader.Application.Read.Contracts;
using WowPaperTrader.Application.Read.Entities;

namespace WowPaperTrader.Application.Read.Interfaces;

public interface ICommodityAuctionRepository
{
    Task<IngestionRun> CreateIngestionRunAsync(CancellationToken cancellationToken);

    Task SaveSnapshotAsync(
        IngestionRun run,
        WowApiResult<AuctionSnapshot> wowApiResult,
        CancellationToken cancellationToken);
}