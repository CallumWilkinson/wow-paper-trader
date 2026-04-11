using Dapper;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Domain.Features.Read.GetTooltip;

namespace WowPaperTrader.Persistence.ReadServices;

public sealed class ItemMetadataAndPriceReadService : IItemMetadataAndPriceReadService
{
    private readonly ApplicationDbContext _dbContext;

    public ItemMetadataAndPriceReadService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TooltipResponse?> GetAsync(long itemId, CancellationToken cancellationToken)
    {
        const string sql =
            """
            WITH LatestSnapshot AS
            (
                SELECT MAX(FetchedAtUtc) AS FetchedAtUtc
                FROM CommodityAuctionSnapshots
            ),
            LowestPriceForItem AS
            (
                SELECT
                    auction.ItemId,
                    MIN(auction.UnitPrice) AS UnitPrice
                FROM CommodityAuctions auction
                INNER JOIN CommodityAuctionSnapshots snapshot
                    ON snapshot.Id = auction.CommodityAuctionSnapshotId
                WHERE auction.ItemId = @ItemId
                AND snapshot.FetchedAtUtc = (SELECT FetchedAtUtc FROM LatestSnapshot)
                GROUP BY auction.ItemId
            )
            SELECT
                price.ItemId,
                price.UnitPrice,
                latest.FetchedAtUtc AS PriceTakenAtUtc,

                meta.Id,
                meta.ItemId,
                meta.Name,
                meta.QualityType,
                meta.QualityName,
                meta.Level,
                meta.RequiredLevel,
                meta.ItemClassId,
                meta.ItemClassName,
                meta.ItemSubclassId,
                meta.ItemSubclassName,
                meta.ProfessionId,
                meta.ProfessionName,
                meta.ProfessionSkillLevel,
                meta.SkillDisplayString,
                meta.CraftingReagent,
                meta.InventoryType,
                meta.InventoryTypeName,
                meta.PurchasePrice,
                meta.SellPrice,
                meta.MaxCount,
                meta.IsEquippable,
                meta.IsStackable,
                meta.PurchaseQuantity,
                meta.ImageUrl,
                meta.LastFetchedUtc
            FROM LowestPriceForItem price
            CROSS JOIN LatestSnapshot latest
            LEFT JOIN ItemMetaData meta
                ON meta.ItemId = price.ItemId;
            """;

        var connection = _dbContext.Database.GetDbConnection();

        var command = new CommandDefinition(sql, new { itemId }, cancellationToken: cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<TooltipResponse>(command);
    }
}