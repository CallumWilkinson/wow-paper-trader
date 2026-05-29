using Dapper;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Application.Features.Write.Helpers;

namespace WowPaperTrader.Persistence.Helpers;

public sealed class DatabaseSizeGuard(ApplicationDbContext dbContext) : IDatabaseSizeGuard
{
    private const decimal MaxDatabaseSizeGb = 28m;
    
    public async Task<bool> IsDatabaseAboveAzureFreeLimit(CancellationToken cancellationToken)
    {
        var databaseSizeGb = await GetDatabaseSizeGbAsync(cancellationToken);

        if (databaseSizeGb >= MaxDatabaseSizeGb)
        {
            return true;
        }
        return false;
    }

    private async Task<decimal> GetDatabaseSizeGbAsync(CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT CAST(SUM(size) * 8.0 / 1024.0 / 1024.0 AS decimal(18, 2))
                           FROM sys.database_files;
                           """;
        var connection = dbContext.Database.GetDbConnection();

        var command = new CommandDefinition(
            commandText: sql,
            cancellationToken: cancellationToken);
        
        return await connection.QuerySingleAsync<decimal>(command);

    }
}