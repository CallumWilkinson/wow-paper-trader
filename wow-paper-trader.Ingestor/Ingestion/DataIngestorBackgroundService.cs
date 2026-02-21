namespace wow_paper_trader.Ingestor;

public sealed class DataIngestorBackgroundService : BackgroundService
{
    private static readonly TimeSpan LoopDelay = TimeSpan.FromHours(1);

    private readonly IServiceScopeFactory _scopeFactory;

    public DataIngestorBackgroundService(
        IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();
                var ingestionRunOrchestrator = scope.ServiceProvider.GetRequiredService<IngestionRunOrchestrator>();
                await ingestionRunOrchestrator.RunOnceAsync(cancellationToken);
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
