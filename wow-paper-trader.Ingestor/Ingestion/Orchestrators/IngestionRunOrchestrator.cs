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
        cancellationToken.Register(() =>
        {
            Console.Error.WriteLine("You have cancelled the program in the middle of an Ingestion Run. You may have to wait upto 10 mins for graceful shutdown");
        });

        var run = new IngestionRun();
        _dbContext.IngestionRuns.Add(run);
        await _dbContext.SaveChangesAsync(cancellationToken);

        try
        {
            await RunCommodityAuctionSnapshotDatabaseTransaction(run, cancellationToken);
            _logger.LogInformation("All Auctions have been recorded in the database successfully!");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        catch (Exception ex)
        {
            _dbContext.IngestionRuns.Attach(run);
            run.MarkFailed(ex, DateTime.UtcNow);

            await _dbContext.SaveChangesAsync(CancellationToken.None);
            _logger.LogError(ex, "Ingestion run failed. RunId={RunId}", run.Id);

            return;
        }
        finally
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _dbContext.IngestionRuns.Attach(run);
                run.TransitionTo(IngestionRunStatus.Cancelled, DateTime.UtcNow);

                await _dbContext.SaveChangesAsync(CancellationToken.None);
                _logger.LogError("Ingestion run cancelled. RunId={RunId}", run.Id);
            }
            _logger.LogInformation("Inserted IngestionRun row at {Time}", DateTimeOffset.Now);
        }



    }


    private async Task RunCommodityAuctionSnapshotDatabaseTransaction(IngestionRun run, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var tokenRequestedAt = DateTime.UtcNow;
            run.TransitionTo(IngestionRunStatus.TokenRequested, tokenRequestedAt);

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

            var startingAdd = DateTime.UtcNow;
            _logger.LogInformation("Adding to DbContext at {Time}", startingAdd);
            _dbContext.CommodityAuctionSnapshots.Add(snapshotEntity);
            _logger.LogInformation("DbContext Add took {Seconds} Seconds", (DateTime.UtcNow - startingAdd).TotalSeconds);

            run.TransitionTo(IngestionRunStatus.Finished, apiResult.DataReturnedAtUtc);

            var startingSaveToDb = DateTime.UtcNow;
            _logger.LogInformation("Starting SQL Write at {Time}", startingSaveToDb);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("SQL Write took {Seconds} Seconds", (DateTime.UtcNow - startingSaveToDb).TotalSeconds);

            await transaction.CommitAsync(cancellationToken);

        }
        catch (Exception)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            _dbContext.ChangeTracker.Clear();

            //throw back up to caller's try catch
            throw;

        }

    }

}