namespace wow_paper_trader.Ingestor;

public sealed class AuctionSnapshotIngestor : BackgroundService
{
    private static readonly TimeSpan LoopDelay = TimeSpan.FromSeconds(5);

    private readonly ILogger<AuctionSnapshotIngestor> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    public AuctionSnapshotIngestor(ILogger<AuctionSnapshotIngestor> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<IngestorDbContext>();

            var run = new IngestionRun { Status = "HeartbeatInsert" };
            db.IngestionRuns.Add(run);
            await db.SaveChangesAsync(stoppingToken);

            _logger.LogInformation("Inserted IngestionRun row at {Time}", DateTimeOffset.Now);

            await Task.Delay(LoopDelay, stoppingToken);
        }
    }
}
