using Dapper;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Application.Features.Read.GetMetadata;

namespace WowPaperTrader.Persistence.ReadServices;

public sealed class MetadataReadService : IMetadataReadService
{
    private readonly ApplicationDbContext _dbContext;

    public MetadataReadService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MetadataResponse?> GetAsync(long itemId, CancellationToken cancellationToken)
    {
        const string sql =
            """
            WITH "LatestSnapshot" AS
            (
                SELECT MAX(snapshot."FetchedAtUtc") AS "FetchedAtUtc"
                FROM public."CommodityAuctionSnapshots" AS snapshot
            ),
            "LowestPriceForItem" AS
            (
                SELECT
                    auction."ItemId",
                    MIN(auction."UnitPrice") AS "UnitPrice"
                FROM public."CommodityAuctions" AS auction
                INNER JOIN public."CommodityAuctionSnapshots" AS snapshot
                    ON snapshot."Id" = auction."CommodityAuctionSnapshotId"
                WHERE auction."ItemId" = @ItemId
                  AND snapshot."FetchedAtUtc" = (
                      SELECT latest."FetchedAtUtc"
                      FROM "LatestSnapshot" AS latest
                  )
                GROUP BY auction."ItemId"
            )
            SELECT
                price."ItemId",
                price."UnitPrice",
                latest."FetchedAtUtc" AS "PriceTakenAtUtc",

                meta."Id",
                meta."ItemId",
                meta."Name",
                meta."QualityType",
                meta."QualityName",
                meta."Level",
                meta."RequiredLevel",
                meta."ItemClassId",
                meta."ItemClassName",
                meta."ItemSubclassId",
                meta."ItemSubclassName",
                meta."ProfessionId",
                meta."ProfessionName",
                meta."ProfessionSkillLevel",
                meta."SkillDisplayString",
                meta."CraftingReagent",
                meta."InventoryType",
                meta."InventoryTypeName",
                meta."PurchasePrice",
                meta."SellPrice",
                meta."MaxCount",
                meta."IsEquippable",
                meta."IsStackable",
                meta."PurchaseQuantity",
                meta."ImageUrl",
                meta."LastFetchedUtc"
            FROM "LowestPriceForItem" AS price
            CROSS JOIN "LatestSnapshot" AS latest
            LEFT JOIN public."ItemMetaData" AS meta
                ON meta."ItemId" = price."ItemId";
            """;

        var connection = _dbContext.Database.GetDbConnection();

        var command = new CommandDefinition(sql, new { itemId }, cancellationToken: cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<MetadataResponse>(command);
    }
}