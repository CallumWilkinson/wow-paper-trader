using Microsoft.Extensions.Logging;
using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;
using WowPaperTrader.Persistence.EntityMappers;

namespace WowPaperTrader.Persistence.Repositories;

public class CommodityAuctionRepository : ICommodityAuctionRepository
{
    public readonly ApplicationDbContext _dbContext;
    public readonly ILogger<CommodityAuctionRepository> _logger;

    public CommodityAuctionRepository(
        ApplicationDbContext dbContext,
        ILogger<CommodityAuctionRepository> logger
    )
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IngestionRun> CreateIngestionRunAsync(CancellationToken cancellationToken)
    {
        var run = new IngestionRun();
        _dbContext.IngestionRuns.Add(run);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return run;
    }

    public async Task SaveSnapshotAsync(IngestionRun run, WowApiResponse<AuctionSnapshot> wowApiResponse,
        CancellationToken cancellationToken)
    {
        var cancellationRegistration =
            cancellationToken.Register(() =>
            {
                Console.Error.WriteLine(
                    "You have cancelled the program in the middle of an Ingestion Run. " +
                    "You may have to wait up to 10 minutes for graceful shutdown."
                );
            });

        try
        {
            await RunCommodityAuctionSnapshotDatabaseTransaction(run, wowApiResponse, cancellationToken);

            _logger.LogInformation("All Auctions have been recorded in the database successfully!");
        }

        catch (Exception ex)
        {
            await MarkRunFailedAsync(run, ex);
            throw;
        }

        finally
        {
            if (cancellationToken.IsCancellationRequested) await MarkRunCancelledAsync(run);

            cancellationRegistration.Dispose();
        }
    }

    private async Task RunCommodityAuctionSnapshotDatabaseTransaction(IngestionRun run,
        WowApiResponse<AuctionSnapshot> wowApiResponse, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var snapshotEntity = CommodityAuctionSnapshotMapper.MapToEntity(wowApiResponse, run.Id);

            var startingAdd = DateTime.UtcNow;
            _logger.LogInformation("Adding to DbContext at {Time}", startingAdd);
            _dbContext.CommodityAuctionSnapshots.Add(snapshotEntity);
            _logger.LogInformation("DbContext Add took {Seconds} Seconds",
                (DateTime.UtcNow - startingAdd).TotalSeconds);

            var startingSaveToDb = DateTime.UtcNow;
            _logger.LogInformation("Starting SQL Write at {Time}", startingSaveToDb);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("SQL Write took {Seconds} Seconds",
                (DateTime.UtcNow - startingSaveToDb).TotalSeconds);

            run.TransitionTo(IngestionRunStatus.Finished, DateTime.UtcNow);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            _dbContext.ChangeTracker.Clear();

            throw;
        }
    }

    private async Task MarkRunFailedAsync(IngestionRun run, Exception exception)
    {
        _dbContext.IngestionRuns.Attach(run);
        run.MarkFailed(exception, DateTime.UtcNow);

        await _dbContext.SaveChangesAsync(CancellationToken.None);
        _logger.LogError(exception, "Ingesion Run Failed. IngestionRunId={RunId}", run.Id);
    }

    private async Task MarkRunCancelledAsync(IngestionRun run)
    {
        _dbContext.IngestionRuns.Attach(run);
        run.TransitionTo(IngestionRunStatus.Cancelled, DateTime.UtcNow);

        await _dbContext.SaveChangesAsync(CancellationToken.None);
        _logger.LogError("Ingestion Run Cancelled. RunId={RunId}", run.Id);
    }
}