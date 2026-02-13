namespace wow_paper_trader.Ingestor;

public sealed class AuctionSnapshotIngestor : BackgroundService
{
    //3600 sec is 1 hour
    private static readonly TimeSpan LoopDelay = TimeSpan.FromSeconds(3600);

    private readonly ILogger<AuctionSnapshotIngestor> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    //created by DI to access local user secrets for the blizzard auth api
    private readonly IConfiguration _config;

    private readonly HttpClient _httpClient;

    public AuctionSnapshotIngestor(ILogger<AuctionSnapshotIngestor> logger, IServiceScopeFactory scopeFactory, IConfiguration config)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _config = config;

        _httpClient = new HttpClient(new SocketsHttpHandler
        {
            //this is to fix "stale DNS issues"
            //I am doing this as i dont want to add a IHttpClientFactory yet
            PooledConnectionLifetime = TimeSpan.FromMinutes(10)
        });
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<IngestorDbContext>();

            IngestionRun run = new IngestionRun();

            //save to db before we call api to assist debugging
            db.IngestionRuns.Add(run);
            await db.SaveChangesAsync(stoppingToken);

            try
            {
                DateTime tokenRequestedAt = DateTime.UtcNow;
                run.TransitionTo(IngestionRunStatus.TokenRequested, tokenRequestedAt);
                await db.SaveChangesAsync(stoppingToken);


                BattleNetAuthClient authClient = new BattleNetAuthClient(_config, _httpClient);
                string? accessToken = await authClient.RequestNewTokenAsync(stoppingToken);

                if (accessToken == null)
                {
                    throw new InvalidOperationException("Access token is null. OAuth token acquisition likely failed.");
                }

                WowApiClient wowApiClient = new WowApiClient(_httpClient);
                string wowRetailCommoditiesEndPoint = "https://us.api.blizzard.com/data/wow/auctions/commodities?namespace=dynamic-us&locale=en_US";
                CommodityAuctionsResponseDto responseDto = await wowApiClient.GetAsync<CommodityAuctionsResponseDto>(wowRetailCommoditiesEndPoint, accessToken, stoppingToken);
                DateTime finishedAtUtc = DateTime.UtcNow;

                CommodityAuctionSnapshotMapper mapper = new CommodityAuctionSnapshotMapper();
                CommodityAuctionSnapshot snapshotEntity = mapper.MapToEntityFromDto(responseDto, run.Id, finishedAtUtc, wowRetailCommoditiesEndPoint);

                db.CommodityAuctionSnapshots.Add(snapshotEntity);
                await db.SaveChangesAsync(stoppingToken);


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
                await db.SaveChangesAsync(stoppingToken);

                _logger.LogError(ex, "Ingestion run failed. RunId={RunId}", run.Id);

            }

            _logger.LogInformation("Inserted IngestionRun row at {Time}", DateTimeOffset.Now);

            await Task.Delay(LoopDelay, stoppingToken);

            //TEMPORARY break means I only run this once, in production I will use loop delay at 2 hours to update DB at interval
            //for now I will just be running ingestor manually once each time
            break;
        }

    }

    //temp fix so no IHttpClientFactory is needed yet
    public override void Dispose()
    {
        _httpClient.Dispose();
        base.Dispose();
    }
}
