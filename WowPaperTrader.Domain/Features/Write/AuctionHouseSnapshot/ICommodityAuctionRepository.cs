using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot.WowApiResult;

namespace WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;

public interface ICommodityAuctionRepository
{
    Task<IngestionRunEntity> CreateIngestionRunAsync(CancellationToken cancellationToken);

    Task SaveSnapshotAsync(
        IngestionRunEntity runEntity,
        WowApiResult<AuctionSnapshot> wowApiResult,
        CancellationToken cancellationToken);
}