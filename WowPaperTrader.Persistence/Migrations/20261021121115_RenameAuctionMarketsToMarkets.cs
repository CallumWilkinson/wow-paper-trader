using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WowPaperTrader.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameAuctionMarketsToMarkets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "AuctionMarkets",
                newName: "Markets");

            migrationBuilder.RenameTable(
                name: "AuctionMarketSnapshots",
                newName: "MarketSnapshots");

            migrationBuilder.RenameColumn(
                name: "AuctionMarketType",
                table: "Markets",
                newName: "MarketType");

            migrationBuilder.RenameColumn(
                name: "AuctionMarketId",
                table: "MarketSnapshots",
                newName: "MarketId");

            migrationBuilder.RenameColumn(
                name: "AuctionMarketId",
                table: "CurrentPriceLevels",
                newName: "MarketId");

            migrationBuilder.RenameColumn(
                name: "AuctionMarketId",
                table: "ItemMarketSnapshots",
                newName: "MarketId");

            migrationBuilder.RenameIndex(
                name: "UX_AuctionMarkets_Region_ConnectedRealmId",
                table: "Markets",
                newName: "UX_Markets_Region_ConnectedRealmId");

            migrationBuilder.RenameIndex(
                name: "UX_AuctionMarkets_Region_RegionalCommodities",
                table: "Markets",
                newName: "UX_Markets_Region_RegionalCommodities");

            migrationBuilder.RenameIndex(
                name: "IX_AuctionMarketSnapshots_IngestionRunId",
                table: "MarketSnapshots",
                newName: "IX_MarketSnapshots_IngestionRunId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentPriceLevels_AuctionMarketId_ObservedAtUtc",
                table: "CurrentPriceLevels",
                newName: "IX_CurrentPriceLevels_MarketId_ObservedAtUtc");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMarketSnapshots_AuctionMarketId_ObservedAtUtc",
                table: "ItemMarketSnapshots",
                newName: "IX_ItemMarketSnapshots_MarketId_ObservedAtUtc");

            migrationBuilder.Sql(
                """
                ALTER TABLE "Markets"
                    RENAME CONSTRAINT "PK_AuctionMarkets" TO "PK_Markets";

                ALTER TABLE "Markets"
                    RENAME CONSTRAINT "CK_AuctionMarkets_AuctionMarketType_ConnectedRealmId"
                    TO "CK_Markets_MarketType_ConnectedRealmId";

                ALTER TABLE "Markets"
                    RENAME CONSTRAINT "AuctionMarkets_Id_not_null" TO "Markets_Id_not_null";

                ALTER TABLE "Markets"
                    RENAME CONSTRAINT "AuctionMarkets_Region_not_null" TO "Markets_Region_not_null";

                ALTER TABLE "Markets"
                    RENAME CONSTRAINT "AuctionMarkets_AuctionMarketType_not_null" TO "Markets_MarketType_not_null";

                ALTER TABLE "Markets"
                    RENAME CONSTRAINT "AuctionMarkets_DisplayName_not_null" TO "Markets_DisplayName_not_null";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "PK_AuctionMarketSnapshots" TO "PK_MarketSnapshots";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "FK_AuctionMarketSnapshots_AuctionMarkets_AuctionMarketId"
                    TO "FK_MarketSnapshots_Markets_MarketId";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "FK_AuctionMarketSnapshots_IngestionRuns_IngestionRunId"
                    TO "FK_MarketSnapshots_IngestionRuns_IngestionRunId";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "AuctionMarketSnapshot_AuctionMarketId_not_null"
                    TO "MarketSnapshots_MarketId_not_null";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "AuctionMarketSnapshots_ObservedAtUtc_not_null"
                    TO "MarketSnapshots_ObservedAtUtc_not_null";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "AuctionMarketSnapshot_FetchedAtUtc_not_null"
                    TO "MarketSnapshots_FetchedAtUtc_not_null";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "AuctionMarketSnapshot_IngestionRunId_not_null"
                    TO "MarketSnapshots_IngestionRunId_not_null";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "AuctionMarketSnapshot_ApiEndPoint_not_null"
                    TO "MarketSnapshots_ApiEndPoint_not_null";

                ALTER TABLE "CurrentPriceLevels"
                    RENAME CONSTRAINT "FK_CurrentPriceLevels_AuctionMarketSnapshots_AuctionMarketId_O~"
                    TO "FK_CurrentPriceLevels_MarketSnapshots_MarketId_O~";

                ALTER TABLE "CurrentPriceLevels"
                    RENAME CONSTRAINT "CurrentPriceLevel_AuctionMarketSnapshotAuctionMarketId_not_null"
                    TO "CurrentPriceLevels_MarketId_not_null";

                ALTER TABLE "CurrentPriceLevels"
                    RENAME CONSTRAINT "CurrentPriceLevel_AuctionMarketSnapshotObservedAtUtc_not_null"
                    TO "CurrentPriceLevels_ObservedAtUtc_not_null";

                ALTER TABLE "ItemMarketSnapshots"
                    RENAME CONSTRAINT "FK_ItemMarketSnapshots_AuctionMarketSnapshots_AuctionMarketId_~"
                    TO "FK_ItemMarketSnapshots_MarketSnapshots_MarketId_~";

                ALTER TABLE "ItemMarketSnapshots"
                    RENAME CONSTRAINT "ItemMarketSnapshot_AuctionMarketSnapshotId_not_null"
                    TO "ItemMarketSnapshots_MarketId_not_null";

                ALTER SEQUENCE "AuctionMarkets_Id_seq" RENAME TO "Markets_Id_seq";
                """);

            migrationBuilder.Sql(
                """
                SELECT setval(
                    '"Markets_Id_seq"',
                    GREATEST(
                        (SELECT COALESCE(MAX("Id"), 1) FROM "Markets"),
                        (SELECT last_value FROM "Markets_Id_seq")
                    ),
                    true
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE "Markets"
                    RENAME CONSTRAINT "PK_Markets" TO "PK_AuctionMarkets";

                ALTER TABLE "Markets"
                    RENAME CONSTRAINT "CK_Markets_MarketType_ConnectedRealmId"
                    TO "CK_AuctionMarkets_AuctionMarketType_ConnectedRealmId";

                ALTER TABLE "Markets"
                    RENAME CONSTRAINT "Markets_Id_not_null" TO "AuctionMarkets_Id_not_null";

                ALTER TABLE "Markets"
                    RENAME CONSTRAINT "Markets_Region_not_null" TO "AuctionMarkets_Region_not_null";

                ALTER TABLE "Markets"
                    RENAME CONSTRAINT "Markets_MarketType_not_null" TO "AuctionMarkets_AuctionMarketType_not_null";

                ALTER TABLE "Markets"
                    RENAME CONSTRAINT "Markets_DisplayName_not_null" TO "AuctionMarkets_DisplayName_not_null";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "PK_MarketSnapshots" TO "PK_AuctionMarketSnapshots";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "FK_MarketSnapshots_Markets_MarketId"
                    TO "FK_AuctionMarketSnapshots_AuctionMarkets_AuctionMarketId";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "FK_MarketSnapshots_IngestionRuns_IngestionRunId"
                    TO "FK_AuctionMarketSnapshots_IngestionRuns_IngestionRunId";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "MarketSnapshots_MarketId_not_null"
                    TO "AuctionMarketSnapshot_AuctionMarketId_not_null";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "MarketSnapshots_ObservedAtUtc_not_null"
                    TO "AuctionMarketSnapshots_ObservedAtUtc_not_null";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "MarketSnapshots_FetchedAtUtc_not_null"
                    TO "AuctionMarketSnapshot_FetchedAtUtc_not_null";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "MarketSnapshots_IngestionRunId_not_null"
                    TO "AuctionMarketSnapshot_IngestionRunId_not_null";

                ALTER TABLE "MarketSnapshots"
                    RENAME CONSTRAINT "MarketSnapshots_ApiEndPoint_not_null"
                    TO "AuctionMarketSnapshot_ApiEndPoint_not_null";

                ALTER TABLE "CurrentPriceLevels"
                    RENAME CONSTRAINT "FK_CurrentPriceLevels_MarketSnapshots_MarketId_O~"
                    TO "FK_CurrentPriceLevels_AuctionMarketSnapshots_AuctionMarketId_O~";

                ALTER TABLE "CurrentPriceLevels"
                    RENAME CONSTRAINT "CurrentPriceLevels_MarketId_not_null"
                    TO "CurrentPriceLevel_AuctionMarketSnapshotAuctionMarketId_not_null";

                ALTER TABLE "CurrentPriceLevels"
                    RENAME CONSTRAINT "CurrentPriceLevels_ObservedAtUtc_not_null"
                    TO "CurrentPriceLevel_AuctionMarketSnapshotObservedAtUtc_not_null";

                ALTER TABLE "ItemMarketSnapshots"
                    RENAME CONSTRAINT "FK_ItemMarketSnapshots_MarketSnapshots_MarketId_~"
                    TO "FK_ItemMarketSnapshots_AuctionMarketSnapshots_AuctionMarketId_~";

                ALTER TABLE "ItemMarketSnapshots"
                    RENAME CONSTRAINT "ItemMarketSnapshots_MarketId_not_null"
                    TO "ItemMarketSnapshot_AuctionMarketSnapshotId_not_null";

                ALTER SEQUENCE "Markets_Id_seq" RENAME TO "AuctionMarkets_Id_seq";
                """);

            migrationBuilder.RenameIndex(
                name: "UX_Markets_Region_ConnectedRealmId",
                table: "Markets",
                newName: "UX_AuctionMarkets_Region_ConnectedRealmId");

            migrationBuilder.RenameIndex(
                name: "UX_Markets_Region_RegionalCommodities",
                table: "Markets",
                newName: "UX_AuctionMarkets_Region_RegionalCommodities");

            migrationBuilder.RenameIndex(
                name: "IX_MarketSnapshots_IngestionRunId",
                table: "MarketSnapshots",
                newName: "IX_AuctionMarketSnapshots_IngestionRunId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentPriceLevels_MarketId_ObservedAtUtc",
                table: "CurrentPriceLevels",
                newName: "IX_CurrentPriceLevels_AuctionMarketId_ObservedAtUtc");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMarketSnapshots_MarketId_ObservedAtUtc",
                table: "ItemMarketSnapshots",
                newName: "IX_ItemMarketSnapshots_AuctionMarketId_ObservedAtUtc");

            migrationBuilder.RenameColumn(
                name: "MarketType",
                table: "Markets",
                newName: "AuctionMarketType");

            migrationBuilder.RenameColumn(
                name: "MarketId",
                table: "MarketSnapshots",
                newName: "AuctionMarketId");

            migrationBuilder.RenameColumn(
                name: "MarketId",
                table: "CurrentPriceLevels",
                newName: "AuctionMarketId");

            migrationBuilder.RenameColumn(
                name: "MarketId",
                table: "ItemMarketSnapshots",
                newName: "AuctionMarketId");

            migrationBuilder.RenameTable(
                name: "MarketSnapshots",
                newName: "AuctionMarketSnapshots");

            migrationBuilder.RenameTable(
                name: "Markets",
                newName: "AuctionMarkets");
        }
    }
}
