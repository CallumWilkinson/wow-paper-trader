using Dapper;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Application.Features.Read.MonthlyPriceQuantity;

namespace WowPaperTrader.Persistence.ReadServices;

public class MonthlyPriceQuantityReadService(ApplicationDbContext dbContext) : IMonthlyPriceQuantityReadService
{
    public async Task<MonthlyPriceQuantityResponse> GetAsync(long itemId, CancellationToken cancellationToken)
    {
        const string sql = 
            """
            SELECT
                ca.CommodityAuctionSnapshotId,
                s.FetchedAtUtc,
                MIN(ca.UnitPrice) AS LowestUnitPrice,
                SUM(ca.Quantity) AS TotalQuantityPosted
            FROM dbo.CommodityAuctions AS ca
            INNER JOIN dbo.CommodityAuctionSnapshots AS s
                ON s.Id = ca.CommodityAuctionSnapshotId
            WHERE ca.ItemId = @ItemId
              AND s.FetchedAtUtc >= DATEADD(DAY, -30, SYSUTCDATETIME())
            GROUP BY
                ca.CommodityAuctionSnapshotId,
                s.FetchedAtUtc
            ORDER BY
                s.FetchedAtUtc DESC;
            """;

        var connection = dbContext.Database.GetDbConnection();
        
        var command = new CommandDefinition(sql, new {itemId}, cancellationToken: cancellationToken);
        
        var list = (await connection.QueryAsync<PriceQuantityResponse>(command)).ToList();

        return new MonthlyPriceQuantityResponse
        {
            ItemId = itemId,
            PriceQuantityResponses = list
        };
    }
}