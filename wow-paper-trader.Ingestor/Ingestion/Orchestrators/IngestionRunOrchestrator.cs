public sealed class IngestionRunOrchestrator
{
    private readonly ILogger<IngestionRunOrchestrator> _logger;
    private readonly IngestorDbContext _dbContext;

    private readonly BattleNetAuthClient _authClient;

    private readonly WowApiClient _wowApiClient;

    public IngestionRunOrchestrator(
    ILogger<IngestionRunOrchestrator> logger,
    IngestorDbContext dbContext,
    BattleNetAuthClient authClient,
    WowApiClient wowApiClient)
    {
        _logger = logger;
        _dbContext = dbContext;
        _authClient = authClient;
        _wowApiClient = wowApiClient;
    }

    public async Task RunOnceAsync(CancellationToken cancellationToken)
    {
        var run = new IngestionRun();

        _dbContext.IngestionRuns.Add(run);
        await _dbContext.SaveChangesAsync(cancellationToken);

        try
        {
            var tokenRequestedAt = DateTime.UtcNow;
            run.TransitionTo(IngestionRunStatus.TokenRequested, tokenRequestedAt);
            await _dbContext.SaveChangesAsync(cancellationToken);

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

            _dbContext.CommodityAuctionSnapshots.Add(snapshotEntity);

            var startingSaveToDb = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("SaveChanges took {Seconds} Seconds", (DateTime.UtcNow - startingSaveToDb).TotalSeconds);


            run.TransitionTo(IngestionRunStatus.Finished, apiResult.DataReturnedAtUtc);
            await _dbContext.SaveChangesAsync(cancellationToken);
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
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogError(ex, "Ingestion run failed. RunId={RunId}", run.Id);
            return;

        }

        _logger.LogInformation("Inserted IngestionRun row at {Time}", DateTimeOffset.Now);

    }

}