using WowPaperTrader.Application.Features.Write.UpdateItems;

namespace WowPaperTrader.Ingestor;

public sealed class UpdateItemMetadataJob (
    IServiceScopeFactory scopeFactory,
    ILogger<UpdateItemMetadataJob> logger)
{
    private static readonly TimeSpan JobTimeout = TimeSpan.FromMinutes(50);
    
    public async Task<int> RunAsync(CancellationToken stoppingToken)
    {
        using var timeoutSource = new CancellationTokenSource(JobTimeout);
        
        using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, timeoutSource.Token);
        
        var cancellationToken = linkedTokenSource.Token;

        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();

            var commandHandler = scope.ServiceProvider.GetRequiredService<UpdateItemsCommandHandler>();

            var command = new UpdateItemsCommand();

            logger.LogInformation("Starting daily item metadata update.");

            await commandHandler.HandleAsync(
                command,
                cancellationToken);

            logger.LogInformation("Daily item metadata update completed.");

            return 0;

        }
        catch (OperationCanceledException exception) when (timeoutSource.IsCancellationRequested)
        {
            logger.LogError(exception, "Item metadata update timed out after {TimeoutMinutes} minutes.",
                JobTimeout.TotalMinutes);

            return 1;
        }
        catch (OperationCanceledException exception)
            when (stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation(
                exception,
                "Item metadata update was cancelled because you pressed Ctrl + C.");

            return 1;
        }
        catch (OperationCanceledException exception)
        {
            logger.LogError(exception, "Item metadata update was cancelled.");

            return 1;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Item metadata update failed.");

            return 1;
        }
        
 
    }
}