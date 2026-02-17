namespace wow_paper_trader.Ingestor;

public sealed class AuctionSnapshotIngestor : BackgroundService
{
    //3600 sec is 1 hour
    private static readonly TimeSpan LoopDelay = TimeSpan.FromSeconds(3600);

    private readonly ILogger<AuctionSnapshotIngestor> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly BattleNetAuthClient _authClient;

    private readonly WowApiClient _wowApiClient;

    public AuctionSnapshotIngestor(
        ILogger<AuctionSnapshotIngestor> logger,
        IServiceScopeFactory scopeFactory,
        BattleNetAuthClient authClient,
        WowApiClient wowApiClient)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _authClient = authClient;
        _wowApiClient = wowApiClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<IngestorDbContext>();

            IngestionRun run = new IngestionRun();

            db.IngestionRuns.Add(run);
            await db.SaveChangesAsync(stoppingToken);

            try
            {
                DateTime tokenRequestedAt = DateTime.UtcNow;
                run.TransitionTo(IngestionRunStatus.TokenRequested, tokenRequestedAt);
                await db.SaveChangesAsync(stoppingToken);

                //this function internally handles 24 hour token expiry logic
                string? accessToken = await _authClient.RequestNewTokenAsync(stoppingToken);

                if (accessToken == null)
                {
                    throw new InvalidOperationException("Access token is null. OAuth token acquisition likely failed.");
                }


                string wowRetailCommoditiesEndPoint = "https://us.api.blizzard.com/data/wow/auctions/commodities?namespace=dynamic-us&locale=en_US";
                CommodityAuctionsResponseDto responseDto = await _wowApiClient.GetAsync<CommodityAuctionsResponseDto>(wowRetailCommoditiesEndPoint, accessToken, stoppingToken);
                DateTime dataReturnedAtUtc = DateTime.UtcNow;

                int auctionsCount = responseDto.CommodityAuctions.Count;
                _logger.LogInformation("Total Auctions Received: {Count}", auctionsCount);

                CommodityAuctionSnapshotMapper mapper = new CommodityAuctionSnapshotMapper();
                CommodityAuctionSnapshot snapshotEntity = mapper.MapToEntityFromDto(responseDto, run.Id, dataReturnedAtUtc, wowRetailCommoditiesEndPoint);

                db.CommodityAuctionSnapshots.Add(snapshotEntity);

                DateTime startingSaveToDb = DateTime.UtcNow;
                await db.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("SaveChanges took {Seconds} Seconds", (DateTime.UtcNow - startingSaveToDb).TotalSeconds);


                run.TransitionTo(IngestionRunStatus.Finished, dataReturnedAtUtc);
                await db.SaveChangesAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                //normal shutdown path, do not mark as failed, this is for user to manually cancel with Ctrl+C
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
}
