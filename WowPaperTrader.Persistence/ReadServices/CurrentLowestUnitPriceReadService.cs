using Dapper;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Domain.Features.Read.LowestPrice;

namespace WowPaperTrader.Persistence.ReadServices;

public sealed class CurrentLowestUnitPriceReadService : ICurrentLowestUnitPriceReadService
{
    private readonly ApplicationDbContext _dbContext;

    public CurrentLowestUnitPriceReadService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CurrentLowestUnitPriceResponse?> GetAsync(long itemId, CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT
                auction.ItemId,
                MIN(auction.UnitPrice) AS UnitPrice,
                snapshot.FetchedAtUtc AS PriceTakenAtUtc
            FROM CommodityAuctionSnapshots snapshot
            INNER JOIN CommodityAuctions auction
                ON auction.CommodityAuctionSnapshotId = snapshot.Id
            WHERE auction.ItemId = @ItemId
            AND snapshot.FetchedAtUtc = (
                SELECT MAX(FetchedAtUtc)
                FROM CommodityAuctionSnapshots
            )
            GROUP BY auction.ItemId, snapshot.FetchedAtUtc
            """;

        var connection = _dbContext.Database.GetDbConnection();

        return await connection.QuerySingleOrDefaultAsync<CurrentLowestUnitPriceResponse>(
            new CommandDefinition(
                sql,
                new { ItemId = itemId },
                cancellationToken: cancellationToken
            )
        );
    }
}