public interface ICommodityAuctionRepository
{
    Task<IngestionRun> CreateIngestionRunAsync(CancellationToken cancellationToken);

    Task SaveSnapshotAsync(
        IngestionRun run,
        CommodityAuctionSnapshot snapshot,
        CancellationToken cancellationToken);

    Task MarkRunFailedAsync(
        IngestionRun run,
        Exception exception);

    Task MarkRunCancelledAsync(
        IngestionRun run);
}