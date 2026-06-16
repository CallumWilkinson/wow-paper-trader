using Dapper;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Application.Features.Read.ItemSearch;

namespace WowPaperTrader.Persistence.ReadServices;

public sealed class ItemSearchReadService(ApplicationDbContext dbContext) : IItemSearchReadService
{
    public async Task<List<ItemSearchResponse>> SearchByNameAsync(string itemName, CancellationToken cancellationToken)
    {
        var search = itemName.Trim();
        
        if (string.IsNullOrWhiteSpace(search))
            return [];
        
        const string sql = """
                           SELECT
                               i."ItemId",
                               i."Name",
                               i."ImageUrl"
                           FROM public."ItemMetaData" AS i
                           WHERE i."Name" ILIKE @ContainsPattern
                           ORDER BY
                               CASE
                                   WHEN lower(i."Name") = lower(@Search) THEN 1
                                   WHEN i."Name" ILIKE @PrefixPattern THEN 2
                                   ELSE 3
                               END,
                               char_length(i."Name"),
                               lower(i."Name"),
                               i."Name"
                           LIMIT 5;
                           """;

        var parameters = new
        {
            Search = search,
            ContainsPattern = $"%{search}%",
            PrefixPattern = $"{search}%"
        };

        var connection = dbContext.Database.GetDbConnection();

        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        var topFiveResults = await connection.QueryAsync<ItemSearchResponse>(command);

        return topFiveResults.ToList();
    }
}