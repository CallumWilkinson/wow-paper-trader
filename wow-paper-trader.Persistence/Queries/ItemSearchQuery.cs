using Microsoft.EntityFrameworkCore;
using Dapper;

public sealed class ItemSearchQuery : IItemSearchQuery
{
    private readonly ApplicationDbContext _dbContext;

    public ItemSearchQuery(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<ItemSearchResult>> SearchByNameAsync(string ItemName, CancellationToken cancellationToken)
    {
        //case insenstive search
        const string sql = @"
            WITH SearchMatches AS
            (
                SELECT TOP 5
                    ItemId,
                    Name,
                    CASE
                        WHEN Name COLLATE SQL_Latin1_General_CP1_CI_AS = @Name COLLATE SQL_Latin1_General_CP1_CI_AS THEN 1
                        WHEN Name COLLATE SQL_Latin1_General_CP1_CI_AS LIKE @Name + '%' COLLATE SQL_Latin1_General_CP1_CI_AS THEN 2
                        WHEN Name COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + @Name + '%' COLLATE SQL_Latin1_General_CP1_CI_AS THEN 3
                        ELSE 4
                    END AS MatchRank
                FROM ItemMetaData
                WHERE
                    Name COLLATE SQL_Latin1_General_CP1_CI_AS LIKE @Name + '%' COLLATE SQL_Latin1_General_CP1_CI_AS
                    OR Name COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + @Name + '%' COLLATE SQL_Latin1_General_CP1_CI_AS
            )
            SELECT
                ItemId,
                Name
            FROM SearchMatches
            ORDER BY
                MatchRank,
                LEN(Name),
                Name;
        ";

        var connection = _dbContext.Database.GetDbConnection();

        var command = new CommandDefinition(sql, new { Name = ItemName }, cancellationToken: cancellationToken);

        var results = await connection.QueryAsync<ItemSearchResult>(command);

        return results.ToList();
    }
}