using Microsoft.Data.SqlClient;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot;
using WowPaperTrader.Application.Features.Write.Helpers;

namespace WowPaperTrader.Ingestor;

public sealed class AuctionDataIngestionJob(
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<AuctionDataIngestionJob> logger)
{
    private static readonly TimeSpan JobTimeout = TimeSpan.FromMinutes(50);

    public async Task<int> RunAsync()
    {
        using var timeoutSource = new CancellationTokenSource(JobTimeout);
        var cancellationToken = timeoutSource.Token;
 
        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            
            var databaseSizeGuard = scope.ServiceProvider.GetRequiredService<IDatabaseSizeGuard>();
            
            if (IsAzureDatabase() && await databaseSizeGuard.IsDatabaseAboveAzureFreeLimit(cancellationToken))
            {
                logger.LogError("Azure database free limit exceeded, the Ingestion Job has been cancelled");

                return 1;
            }
            
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

    private bool IsAzureDatabase()
    {
        var connectionString = configuration.GetConnectionString("WowPaperTrader");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return false;
        }

        var builder = new SqlConnectionStringBuilder(connectionString);

        if (builder.DataSource.Contains(".database.windows.net", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }
}