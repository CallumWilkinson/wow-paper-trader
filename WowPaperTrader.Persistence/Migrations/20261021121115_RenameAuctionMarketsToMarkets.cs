using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WowPaperTrader.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameAuctionMarketsToMarkets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentPriceLevels_AuctionMarketSnapshots_AuctionMarketId_O~",
                table: "CurrentPriceLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemMarketSnapshots_AuctionMarketSnapshots_AuctionMarketId_~",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropTable(
                name: "AuctionMarketSnapshots");

            migrationBuilder.DropTable(
                name: "AuctionMarkets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentPriceLevels",
                table: "CurrentPriceLevels");

            migrationBuilder.DropIndex(
                name: "IX_CurrentPriceLevels_AuctionMarketId_ObservedAtUtc",
                table: "CurrentPriceLevels");

            migrationBuilder.DropColumn(
                name: "AuctionMarketId",
                table: "CurrentPriceLevels");

            migrationBuilder.RenameColumn(
                name: "AuctionMarketId",
                table: "ItemMarketSnapshots",
                newName: "MarketId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMarketSnapshots_AuctionMarketId_ObservedAtUtc",
                table: "ItemMarketSnapshots",
                newName: "IX_ItemMarketSnapshots_MarketId_ObservedAtUtc");

            migrationBuilder.RenameColumn(
                name: "AuctionMarketSnapshotId",
                table: "CurrentPriceLevels",
                newName: "MarketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentPriceLevels",
                table: "CurrentPriceLevels",
                columns: new[] { "MarketId", "ItemId", "VariantKey", "UnitPrice" });

            migrationBuilder.CreateTable(
                name: "Markets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Region = table.Column<string>(type: "text", nullable: false),
                    MarketType = table.Column<int>(type: "integer", nullable: false),
                    ConnectedRealmId = table.Column<long>(type: "bigint", nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markets", x => x.Id);
                    table.CheckConstraint("CK_Markets_MarketType_ConnectedRealmId", "(\n    (\"MarketType\" = 1 AND \"ConnectedRealmId\" IS NULL)\n    OR\n    (\"MarketType\" = 2 AND \"ConnectedRealmId\" IS NOT NULL)\n)");
                });

            migrationBuilder.CreateTable(
                name: "MarketSnapshots",
                columns: table => new
                {
                    MarketId = table.Column<long>(type: "bigint", nullable: false),
                    ObservedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FetchedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IngestionRunId = table.Column<long>(type: "bigint", nullable: false),
                    ApiEndPoint = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketSnapshots", x => new { x.MarketId, x.ObservedAtUtc });
                    table.ForeignKey(
                        name: "FK_MarketSnapshots_IngestionRuns_IngestionRunId",
                        column: x => x.IngestionRunId,
                        principalTable: "IngestionRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MarketSnapshots_Markets_MarketId",
                        column: x => x.MarketId,
                        principalTable: "Markets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Markets",
                columns: new[] { "Id", "ConnectedRealmId", "DisplayName", "MarketType", "Region" },
                values: new object[] { 1L, null, "US Commodities", 1, "US" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemMarketSnapshots_ObservedAtUtc",
                table: "ItemMarketSnapshots",
                column: "ObservedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentPriceLevels_MarketId_ObservedAtUtc",
                table: "CurrentPriceLevels",
                columns: new[] { "MarketId", "ObservedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "UX_Markets_Region_ConnectedRealmId",
                table: "Markets",
                columns: new[] { "ConnectedRealmId", "Region" },
                unique: true,
                filter: "\"MarketType\" = 2");

            migrationBuilder.CreateIndex(
                name: "UX_Markets_Region_RegionalCommodities",
                table: "Markets",
                column: "Region",
                unique: true,
                filter: "\"MarketType\" = 1");

            migrationBuilder.CreateIndex(
                name: "IX_MarketSnapshots_IngestionRunId",
                table: "MarketSnapshots",
                column: "IngestionRunId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentPriceLevels_MarketSnapshots_MarketId_O~",
                table: "CurrentPriceLevels",
                columns: new[] { "MarketId", "ObservedAtUtc" },
                principalTable: "MarketSnapshots",
                principalColumns: new[] { "MarketId", "ObservedAtUtc" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMarketSnapshots_MarketSnapshots_MarketId_~",
                table: "ItemMarketSnapshots",
                columns: new[] { "MarketId", "ObservedAtUtc" },
                principalTable: "MarketSnapshots",
                principalColumns: new[] { "MarketId", "ObservedAtUtc" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentPriceLevels_MarketSnapshots_MarketId_O~",
                table: "CurrentPriceLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemMarketSnapshots_MarketSnapshots_MarketId_~",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropTable(
                name: "MarketSnapshots");

            migrationBuilder.DropTable(
                name: "Markets");

            migrationBuilder.DropIndex(
                name: "IX_ItemMarketSnapshots_ObservedAtUtc",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentPriceLevels",
                table: "CurrentPriceLevels");

            migrationBuilder.DropIndex(
                name: "IX_CurrentPriceLevels_MarketId_ObservedAtUtc",
                table: "CurrentPriceLevels");

            migrationBuilder.RenameColumn(
                name: "MarketId",
                table: "ItemMarketSnapshots",
                newName: "AuctionMarketId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMarketSnapshots_MarketId_ObservedAtUtc",
                table: "ItemMarketSnapshots",
                newName: "IX_ItemMarketSnapshots_AuctionMarketId_ObservedAtUtc");

            migrationBuilder.RenameColumn(
                name: "MarketId",
                table: "CurrentPriceLevels",
                newName: "AuctionMarketSnapshotId");

            migrationBuilder.AddColumn<long>(
                name: "AuctionMarketId",
                table: "CurrentPriceLevels",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentPriceLevels",
                table: "CurrentPriceLevels",
                columns: new[] { "AuctionMarketId", "ItemId", "VariantKey", "UnitPrice" });

            migrationBuilder.CreateTable(
                name: "AuctionMarkets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuctionMarketType = table.Column<int>(type: "integer", nullable: false),
                    ConnectedRealmId = table.Column<long>(type: "bigint", nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionMarkets", x => x.Id);
                    table.CheckConstraint("CK_AuctionMarkets_AuctionMarketType_ConnectedRealmId", "(\n    (\"AuctionMarketType\" = 1 AND \"ConnectedRealmId\" IS NULL)\n    OR\n    (\"AuctionMarketType\" = 2 AND \"ConnectedRealmId\" IS NOT NULL)\n)");
                });

            migrationBuilder.CreateTable(
                name: "AuctionMarketSnapshots",
                columns: table => new
                {
                    AuctionMarketId = table.Column<long>(type: "bigint", nullable: false),
                    ObservedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IngestionRunId = table.Column<long>(type: "bigint", nullable: false),
                    ApiEndPoint = table.Column<string>(type: "text", nullable: false),
                    FetchedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionMarketSnapshots", x => new { x.AuctionMarketId, x.ObservedAtUtc });
                    table.ForeignKey(
                        name: "FK_AuctionMarketSnapshots_AuctionMarkets_AuctionMarketId",
                        column: x => x.AuctionMarketId,
                        principalTable: "AuctionMarkets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuctionMarketSnapshots_IngestionRuns_IngestionRunId",
                        column: x => x.IngestionRunId,
                        principalTable: "IngestionRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AuctionMarkets",
                columns: new[] { "Id", "AuctionMarketType", "ConnectedRealmId", "DisplayName", "Region" },
                values: new object[] { 1L, 1, null, "US Commodities", "US" });

            migrationBuilder.CreateIndex(
                name: "IX_CurrentPriceLevels_AuctionMarketId_ObservedAtUtc",
                table: "CurrentPriceLevels",
                columns: new[] { "AuctionMarketId", "ObservedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "UX_AuctionMarkets_Region_ConnectedRealmId",
                table: "AuctionMarkets",
                columns: new[] { "ConnectedRealmId", "Region" },
                unique: true,
                filter: "\"AuctionMarketType\" = 2");

            migrationBuilder.CreateIndex(
                name: "UX_AuctionMarkets_Region_RegionalCommodities",
                table: "AuctionMarkets",
                column: "Region",
                unique: true,
                filter: "\"AuctionMarketType\" = 1");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionMarketSnapshots_IngestionRunId",
                table: "AuctionMarketSnapshots",
                column: "IngestionRunId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentPriceLevels_AuctionMarketSnapshots_AuctionMarketId_O~",
                table: "CurrentPriceLevels",
                columns: new[] { "AuctionMarketId", "ObservedAtUtc" },
                principalTable: "AuctionMarketSnapshots",
                principalColumns: new[] { "AuctionMarketId", "ObservedAtUtc" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMarketSnapshots_AuctionMarketSnapshots_AuctionMarketId_~",
                table: "ItemMarketSnapshots",
                columns: new[] { "AuctionMarketId", "ObservedAtUtc" },
                principalTable: "AuctionMarketSnapshots",
                principalColumns: new[] { "AuctionMarketId", "ObservedAtUtc" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
