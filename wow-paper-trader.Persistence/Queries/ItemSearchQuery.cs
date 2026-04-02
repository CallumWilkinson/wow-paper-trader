using Microsoft.EntityFrameworkCore;
using Dapper;

public sealed class ItemSearchQuery : IItemSearchQuery
{
    private readonly ApplicationDbContext _dbContext;

    public ItemSearchQuery(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<ItemSearchResult>> SearchCandidatesByNameAsync(string itemName, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT
                ItemId,
                Name
            FROM ItemMetaData
            WHERE Name LIKE '%' + @Name + '%';
        ";

        var connection = _dbContext.Database.GetDbConnection();

        var command = new CommandDefinition(sql, new { Name = itemName }, cancellationToken: cancellationToken);

        var results = await connection.QueryAsync<ItemSearchResult>(command);

        return results.ToList();

    }

}