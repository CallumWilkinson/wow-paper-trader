public interface ICommodityAuctionRepository
{
    Task<IngestionRun> CreateRunAsync(CancellationToken cancellationToken);
}




// public interface IAuctionRepository
// {
//     Task<IngestionRun> CreateRunAsync(CancellationToken cancellationToken);

//     Task SaveSnapshotAsync(
//         IngestionRun run,
//         AuctionSnapshot snapshot,
//         CancellationToken cancellationToken);

//     Task MarkRunFailedAsync(
//         IngestionRun run,
//         Exception exception);

//     Task MarkRunCancelledAsync(
//         IngestionRun run);
// }