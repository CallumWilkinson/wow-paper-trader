using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;

namespace WowPaperTrader.Ingestor;

public sealed class AuctionDataBackgroundService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    private static readonly TimeSpan LoopDelay = TimeSpan.FromHours(1);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                
                var postAuctionDataCommandHandler = scope.ServiceProvider.GetRequiredService<PostAuctionDataCommandHandler>();
                
                var command = new PostAuctionDataCommand();
                
                await postAuctionDataCommandHandler.HandleAsync(command, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            try
            {
                await Task.Delay(LoopDelay, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }
}