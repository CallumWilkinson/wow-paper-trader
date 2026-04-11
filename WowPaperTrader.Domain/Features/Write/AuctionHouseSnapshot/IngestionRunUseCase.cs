using Microsoft.Extensions.Logging;

namespace WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;

public sealed class IngestionRunUseCase(
    ILogger<IngestionRunUseCase> logger,
    ICommodityAuctionApiAdapter auctionApiAdapter,
    ICommodityAuctionRepository repository)
{
    public async Task RunOnceAsync(CancellationToken cancellationToken)
    {
        var run = await repository.CreateIngestionRunAsync(cancellationToken);

        try
        {
            var result = await auctionApiAdapter.GetCommodityAuctionsSnapshotAsync(cancellationToken);

            await repository.SaveSnapshotAsync(run, result, cancellationToken);

            logger.LogInformation("IngestionRunEntity UseCase completed successfully.");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("IngestionRunEntity UseCase Cancelled");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "IngestionRunEntity UseCase Failed");
            throw;
        }
    }
}