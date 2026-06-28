using Microsoft.Data.SqlClient;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot;

namespace WowPaperTrader.Ingestor;

public sealed class AuctionDataIngestionJob(
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<AuctionDataIngestionJob> logger)
{
    private static readonly TimeSpan JobTimeout = TimeSpan.FromMinutes(50);
    private static readonly TimeSpan DatabaseRetryInterval = TimeSpan.FromSeconds(20);

    private const int MaxDatabaseConnectionAttempts = 5;

    public async Task<int> RunAsync()
    {
        using var timeoutSource = new CancellationTokenSource(JobTimeout);
        var cancellationToken = timeoutSource.Token;
 
        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            
            var postAuctionDataCommandHandler = scope.ServiceProvider.GetRequiredService<PostAuctionDataCommandHandler>();
            
            var command = new PostAuctionDataCommand();
            
            await postAuctionDataCommandHandler.HandleAsync(command, cancellationToken);

            return 0;
        }
        catch (OperationCanceledException exception) when (timeoutSource.IsCancellationRequested)
        {
            logger.LogError(exception, "Auction data ingestion job timed out after {TimeoutMinutes} minutes.", JobTimeout.TotalMinutes);

            return 1;
        }
        catch (OperationCanceledException exception)
        {
            logger.LogError(exception, "Auction data ingestion was cancelled.");

            return 1;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Auction data ingestion failed.");

            return 1;
        }
        
    }

    //originally used to avoid extra azure costs, safe to delete if no longer using any cloud platforms
    private async Task EnsureDatabaseIsReadyAsync(CancellationToken cancellationToken)
    {
        var connectionString = configuration.GetConnectionString("WowPaperTrader");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("WowPaperTrader connection string was not found.");
        }

        for (var attempt = 1; attempt <= MaxDatabaseConnectionAttempts; attempt++)
        {
            try
            {
                await using var connection = new SqlConnection(connectionString);
                
                await connection.OpenAsync(cancellationToken);
                
                logger.LogInformation("Database connection check succeeded.");

                return;
            }
            //sql error 40613 is database not available
            catch (SqlException exception) when (exception.Number == 40613 && attempt < MaxDatabaseConnectionAttempts)
            {
                logger.LogWarning(
                    exception,
                    "Database is not currently available. Retrying in {RetryDelaySeconds} seconds. Attempt {Attempt}/{MaxAttempts}.",
                    DatabaseRetryInterval.TotalSeconds,
                    attempt,
                    MaxDatabaseConnectionAttempts);

                await Task.Delay(DatabaseRetryInterval, cancellationToken);
            }
        }
    }
    
}