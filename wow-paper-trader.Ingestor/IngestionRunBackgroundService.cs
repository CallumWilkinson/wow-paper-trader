namespace wow_paper_trader.Ingestor;

public sealed class IngestionRunBackgroundService : BackgroundService
{
    private static readonly TimeSpan LoopDelay = TimeSpan.FromHours(1);

    private readonly IServiceScopeFactory _scopeFactory;

    public IngestionRunBackgroundService(
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
                var ingestionUseCase = scope.ServiceProvider.GetRequiredService<IngestionRunUseCase>();
                await ingestionUseCase.RunOnceAsync(cancellationToken);
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
