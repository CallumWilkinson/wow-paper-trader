using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot;

namespace WowPaperTrader.Ingestor;

public sealed class AuctionDataIngestionJob(IServiceScopeFactory scopeFactory)
{
    private static readonly TimeSpan JobTimeout = TimeSpan.FromMinutes(50);

    public async Task<int> RunAsync()
    {
        using var timeoutSource = new CancellationTokenSource(JobTimeout);
        var cancellationToken = timeoutSource.Token;
 
        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            
            var postAuctionDataCommandHandler = scope.ServiceProvider.GetRequiredService<PostAuctionDataCommandHandler>();
            
            var command = new PostAuctionDataCommand();
            
            await postAuctionDataCommandHandler.HandleAsync(command, cancellationToken);

            return 0;
        }
        catch (OperationCanceledException)
        {
            Console.Error.WriteLine("Auction data ingestion was cancelled or timed out.");

            return 1;
        }
        catch (Exception)
        {
            Console.Error.WriteLine("Auction data ingestion failed.");

            return 1;
        }
        
    }
}