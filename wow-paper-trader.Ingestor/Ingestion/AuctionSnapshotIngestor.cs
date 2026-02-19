namespace wow_paper_trader.Ingestor;

public sealed class AuctionSnapshotIngestor : BackgroundService
{
    private static readonly TimeSpan LoopDelay = TimeSpan.FromHours(1);

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

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await RunOnceAsync(cancellationToken);

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


    public async Task RunOnceAsync(CancellationToken cancellationToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<IngestorDbContext>();

        var run = new IngestionRun();

        db.IngestionRuns.Add(run);
        await db.SaveChangesAsync(cancellationToken);

        try
        {
            var tokenRequestedAt = DateTime.UtcNow;
            run.TransitionTo(IngestionRunStatus.TokenRequested, tokenRequestedAt);
            await db.SaveChangesAsync(cancellationToken);

            //this function internally handles 24 hour token expiry logic
            string? accessToken = await _authClient.RequestNewTokenAsync(cancellationToken);

            if (accessToken == null)
            {
                throw new InvalidOperationException("Access token is null. OAuth token acquisition likely failed.");
            }

            WowApiClient.WowApiResult<CommodityAuctionsResponseDto> apiResult = await _wowApiClient.GetCommodityAuctionsAsync(accessToken, cancellationToken);

            int auctionsCount = apiResult.Payload.CommodityAuctions.Count;
            _logger.LogInformation("Total Auctions Received: {Count}", auctionsCount);

            var mapper = new CommodityAuctionSnapshotMapper();
            CommodityAuctionSnapshot snapshotEntity = mapper.MapToEntityFromDto(apiResult.Payload, run.Id, apiResult.DataReturnedAtUtc, apiResult.Endpoint);

            db.CommodityAuctionSnapshots.Add(snapshotEntity);

            var startingSaveToDb = DateTime.UtcNow;
            await db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("SaveChanges took {Seconds} Seconds", (DateTime.UtcNow - startingSaveToDb).TotalSeconds);


            run.TransitionTo(IngestionRunStatus.Finished, apiResult.DataReturnedAtUtc);
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            //normal shutdown path, do not mark as failed, this is for user to manually cancel with Ctrl+C
            return;
        }
        catch (Exception ex)
        {
            var failedAt = DateTime.UtcNow;
            run.MarkFailed(ex, failedAt);
            await db.SaveChangesAsync(cancellationToken);

            _logger.LogError(ex, "Ingestion run failed. RunId={RunId}", run.Id);
            return;

        }

        _logger.LogInformation("Inserted IngestionRun row at {Time}", DateTimeOffset.Now);

    }
}
