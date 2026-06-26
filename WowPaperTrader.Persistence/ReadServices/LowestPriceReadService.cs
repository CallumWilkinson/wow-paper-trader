using Dapper;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Application.Features.Read.LowestPrice;

namespace WowPaperTrader.Persistence.ReadServices;

public sealed class LowestPriceReadService : ILowestPriceReadService
{
    private readonly ApplicationDbContext _dbContext;

    public LowestPriceReadService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<LowestPriceResponse?> GetAsync(long itemId, CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT
                auction."ItemId",
                MIN(auction."UnitPrice") AS "UnitPrice",
                snapshot."FetchedAtUtc" AS "PriceTakenAtUtc"
            FROM public."CommodityAuctionSnapshots" AS snapshot
            INNER JOIN public."CommodityAuctions" AS auction
                ON auction."CommodityAuctionSnapshotId" = snapshot."Id"
            WHERE auction."ItemId" = @ItemId
              AND snapshot."FetchedAtUtc" = (
                  SELECT MAX(latest_snapshot."FetchedAtUtc")
                  FROM public."CommodityAuctionSnapshots" AS latest_snapshot
              )
            GROUP BY
                auction."ItemId",
                snapshot."FetchedAtUtc";
            """;

        var connection = _dbContext.Database.GetDbConnection();

        return await connection.QuerySingleOrDefaultAsync<LowestPriceResponse>(
            new CommandDefinition(
                sql,
                new { ItemId = itemId },
                cancellationToken: cancellationToken
            )
        );
    }
}