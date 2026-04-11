namespace WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;

public interface ICommodityAuctionRepository
{
    Task<IngestionRunEntity> CreateIngestionRunAsync(CancellationToken cancellationToken);

    Task SaveSnapshotAsync(
        IngestionRunEntity runEntity,
        WowApiResponse<AuctionSnapshot> wowApiResponse,
        CancellationToken cancellationToken);
}