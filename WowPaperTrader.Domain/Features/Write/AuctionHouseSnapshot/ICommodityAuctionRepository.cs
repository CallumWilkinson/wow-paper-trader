namespace WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;

public interface ICommodityAuctionRepository
{
    Task<IngestionRun> CreateIngestionRunAsync(CancellationToken cancellationToken);

    Task SaveSnapshotAsync(
        IngestionRun run,
        WowApiResponse<AuctionSnapshot> wowApiResponse,
        CancellationToken cancellationToken);
}