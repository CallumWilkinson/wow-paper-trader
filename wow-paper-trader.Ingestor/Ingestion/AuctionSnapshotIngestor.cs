namespace wow_paper_trader.Ingestor;

public sealed class AuctionSnapshotIngestor : BackgroundService
{

    private readonly ILogger<AuctionSnapshotIngestor> _logger;

    public AuctionSnapshotIngestor(ILogger<AuctionSnapshotIngestor> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
