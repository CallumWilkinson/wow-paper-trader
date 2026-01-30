namespace wow_paper_trader.Ingestor;

public sealed class AuctionSnapshotIngestor : BackgroundService
{
    //3600 sec is 1 hour
    private static readonly TimeSpan LoopDelay = TimeSpan.FromSeconds(3600);

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

            var run = new IngestionRun();

            //save to db before we call api to assist debugging
            db.IngestionRuns.Add(run);
            await db.SaveChangesAsync(stoppingToken);

            try
            {
                var tokenRequestedAt = DateTime.UtcNow;
                run.TransitionTo(IngestionRunStatus.TokenRequested, tokenRequestedAt);
                await db.SaveChangesAsync(stoppingToken);

                //TODO: add code to actually call the blizzard api here and save changes again


                var finishedAtUtc = DateTime.UtcNow;
                run.TransitionTo(IngestionRunStatus.Finished, finishedAtUtc);
                await db.SaveChangesAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                //normal shutdown path, do not mark as failed
                break;
            }
            catch (Exception ex)
            {
                var failedAt = DateTime.UtcNow;
                run.MarkFailed(ex, failedAt);

                _logger.LogError(ex, "Ingestion run failed. RunId={RunId}", run.Id);
                await db.SaveChangesAsync(stoppingToken);

            }

            _logger.LogInformation("Inserted IngestionRun row at {Time}", DateTimeOffset.Now);

            await Task.Delay(LoopDelay, stoppingToken);

            //TEMPORARY break means I only run this once, in production I will use loop delay at 2 hours to update DB at interval
            //for now I will just be running ingestor manually once each time
            break;
        }
    }
}
