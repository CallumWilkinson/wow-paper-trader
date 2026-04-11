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

    public async Task<IngestionRunEntity> CreateIngestionRunAsync(CancellationToken cancellationToken)
    {
        var run = new IngestionRunEntity();
        _dbContext.IngestionRuns.Add(run);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return run;
    }

    public async Task SaveSnapshotAsync(IngestionRunEntity runEntity, WowApiResponse<AuctionSnapshot> wowApiResponse,
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
            await RunCommodityAuctionSnapshotDatabaseTransaction(runEntity, wowApiResponse, cancellationToken);

            _logger.LogInformation("All Auctions have been recorded in the database successfully!");
        }

        catch (Exception ex)
        {
            await MarkRunFailedAsync(runEntity, ex);
            throw;
        }

        finally
        {
            if (cancellationToken.IsCancellationRequested) await MarkRunCancelledAsync(runEntity);

            cancellationRegistration.Dispose();
        }
    }

    private async Task RunCommodityAuctionSnapshotDatabaseTransaction(IngestionRunEntity runEntity,
        WowApiResponse<AuctionSnapshot> wowApiResponse, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var snapshotEntity = CommodityAuctionSnapshotMapper.MapToEntity(wowApiResponse, runEntity.Id);

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

            runEntity.TransitionTo(IngestionRunStatus.Finished, DateTime.UtcNow);
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

    private async Task MarkRunFailedAsync(IngestionRunEntity runEntity, Exception exception)
    {
        _dbContext.IngestionRuns.Attach(runEntity);
        runEntity.MarkFailed(exception, DateTime.UtcNow);

        await _dbContext.SaveChangesAsync(CancellationToken.None);
        _logger.LogError(exception, "Ingesion Run Failed. IngestionRunId={RunId}", runEntity.Id);
    }

    private async Task MarkRunCancelledAsync(IngestionRunEntity runEntity)
    {
        _dbContext.IngestionRuns.Attach(runEntity);
        runEntity.TransitionTo(IngestionRunStatus.Cancelled, DateTime.UtcNow);

        await _dbContext.SaveChangesAsync(CancellationToken.None);
        _logger.LogError("Ingestion Run Cancelled. RunId={RunId}", runEntity.Id);
    }
}