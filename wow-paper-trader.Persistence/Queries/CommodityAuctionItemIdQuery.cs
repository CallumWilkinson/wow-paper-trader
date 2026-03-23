using Dapper;
using Microsoft.EntityFrameworkCore;

public sealed class CommodityAuctionItemIdQuery : ICommodityAuctionItemIdQuery
{
    private readonly ApplicationDbContext _dbContext;

    public CommodityAuctionItemIdQuery(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<long>> GetAllUniqueItemIdsAsync(CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT DISTINCT ItemId
            FROM CommodityAuctions;
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