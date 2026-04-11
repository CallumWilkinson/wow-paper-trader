using Dapper;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Application.Features.Write.UpdateItems;

namespace WowPaperTrader.Persistence.ReadServices;

public sealed class ItemIdsWithoutMetadataReadService : IItemIdsWithoutMetadataReadService
{
    private readonly ApplicationDbContext _dbContext;

    public ItemIdsWithoutMetadataReadService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<long>> GetItemIdsWithoutMetadataAsync(CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT DISTINCT ca.ItemId
            FROM CommodityAuctions ca
            WHERE NOT EXISTS (
                SELECT 1
                FROM ItemMetaData im
                WHERE im.ItemId = ca.ItemId
            );
        ";

        var connection = _dbContext.Database.GetDbConnection();

        try
        {
            var result = await connection.QueryAsync<long>(sql);

            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve unique ItemIds from CommodityAuctions", ex);
        }
    }
}