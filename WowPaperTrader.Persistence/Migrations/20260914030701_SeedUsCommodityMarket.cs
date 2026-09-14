using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WowPaperTrader.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsCommodityMarket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuctionMarkets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Region = table.Column<string>(type: "text", nullable: false),
                    AuctionMarketType = table.Column<int>(type: "integer", nullable: false),
                    ConnectedRealmId = table.Column<long>(type: "bigint", nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionMarkets", x => x.Id);
                    table.CheckConstraint("CK_AuctionMarkets_AuctionMarketType_ConnectedRealmId", "(\n    (\"AuctionMarketType\" = 1 AND \"ConnectedRealmId\" IS NULL)\n    OR\n    (\"AuctionMarketType\" = 2 AND \"ConnectedRealmId\" IS NOT NULL)\n)");
                });

            migrationBuilder.CreateTable(
                name: "AuctionMarketSnapshot",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuctionMarketId = table.Column<long>(type: "bigint", nullable: false),
                    FetchedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IngestionRunId = table.Column<long>(type: "bigint", nullable: false),
                    ApiEndPoint = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionMarketSnapshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuctionMarketSnapshot_AuctionMarkets_AuctionMarketId",
                        column: x => x.AuctionMarketId,
                        principalTable: "AuctionMarkets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuctionMarketSnapshot_IngestionRuns_IngestionRunId",
                        column: x => x.IngestionRunId,
                        principalTable: "IngestionRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrentPriceLevel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuctionMarketSnapshotId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    VariantKey = table.Column<long>(type: "bigint", nullable: false),
                    UnitPrice = table.Column<long>(type: "bigint", nullable: false),
                    TotalQuantity = table.Column<long>(type: "bigint", nullable: false),
                    ListingCount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentPriceLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrentPriceLevel_AuctionMarketSnapshot_AuctionMarketSnapsh~",
                        column: x => x.AuctionMarketSnapshotId,
                        principalTable: "AuctionMarketSnapshot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemMarketSnapshot",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuctionMarketSnapshotId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    VariantKey = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemMarketSnapshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemMarketSnapshot_AuctionMarketSnapshot_AuctionMarketSnaps~",
                        column: x => x.AuctionMarketSnapshotId,
                        principalTable: "AuctionMarketSnapshot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AuctionMarkets",
                columns: new[] { "Id", "AuctionMarketType", "ConnectedRealmId", "DisplayName", "Region" },
                values: new object[] { 1L, 1, null, "US Commodities", "US" });

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
                name: "IX_AuctionMarketSnapshot_AuctionMarketId",
                table: "AuctionMarketSnapshot",
                column: "AuctionMarketId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionMarketSnapshot_IngestionRunId",
                table: "AuctionMarketSnapshot",
                column: "IngestionRunId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentPriceLevel_AuctionMarketSnapshotId",
                table: "CurrentPriceLevel",
                column: "AuctionMarketSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMarketSnapshot_AuctionMarketSnapshotId",
                table: "ItemMarketSnapshot",
                column: "AuctionMarketSnapshotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentPriceLevel");

            migrationBuilder.DropTable(
                name: "ItemMarketSnapshot");

            migrationBuilder.DropTable(
                name: "AuctionMarketSnapshot");

            migrationBuilder.DropTable(
                name: "AuctionMarkets");
        }
    }
}
