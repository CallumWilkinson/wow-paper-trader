using Dapper;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Domain.Interfaces;
using WowPaperTrader.Domain.ResponseTypes;

namespace WowPaperTrader.Persistence.Queries;

public sealed class ItemSearchQuery : IItemSearchQuery
{
    private readonly ApplicationDbContext _dbContext;

    public ItemSearchQuery(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ItemSearchResult>> SearchByNameAsync(string itemName, CancellationToken cancellationToken)
    {
        const string sql = @"
            DECLARE @Search nvarchar(4000) = TRIM(@Name);

            SELECT TOP (5)
                i.ItemId,
                i.Name
            FROM dbo.ItemMetaData AS i
            WHERE i.Name COLLATE Latin1_General_100_CI_AS LIKE N'%' + @Search + N'%'
            ORDER BY
                CASE
                    WHEN i.Name COLLATE Latin1_General_100_CI_AS = @Search THEN 1
                    WHEN i.Name COLLATE Latin1_General_100_CI_AS LIKE @Search + N'%' THEN 2
                    ELSE 3
                END,
                LEN(i.Name + N'.') - 1,
                i.Name COLLATE Latin1_General_100_CI_AS;
            ";

        var connection = _dbContext.Database.GetDbConnection();

        var command = new CommandDefinition(sql, new { Name = itemName }, cancellationToken: cancellationToken);

        var topFiveResults = await connection.QueryAsync<ItemSearchResult>(command);

        return topFiveResults.ToList();
    }
}