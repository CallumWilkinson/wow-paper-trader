using Microsoft.Extensions.Logging;
using WowPaperTrader.Domain.Architecture;

namespace WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;

public sealed class PostAuctionDataCommandHandler(
    ILogger<PostAuctionDataCommandHandler> logger,
    ICommodityAuctionApiAdapter auctionApiAdapter,
    ICommodityAuctionRepository repository) : ICommandHandler<PostAuctionDataCommand>
{
    public async Task HandleAsync(PostAuctionDataCommand command, CancellationToken cancellationToken)
    {
        var run = await repository.CreateIngestionRunAsync(cancellationToken);

        try
        {
            var result = await auctionApiAdapter.GetCommodityAuctionsSnapshotAsync(cancellationToken);

            await repository.SaveSnapshotAsync(run, result, cancellationToken);

            logger.LogInformation("Commodity Auction Snapshot completed successfully.");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("Commodity Auction Snapshot Cancelled");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Commodity Auction Snapshot Failed");
            throw;
        }
    }
    
}