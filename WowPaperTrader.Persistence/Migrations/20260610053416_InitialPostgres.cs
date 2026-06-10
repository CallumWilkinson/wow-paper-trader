using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WowPaperTrader.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IngestionRuns",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinishedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    ErrorStack = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngestionRuns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemMetaData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    QualityType = table.Column<string>(type: "text", nullable: true),
                    QualityName = table.Column<string>(type: "text", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    RequiredLevel = table.Column<int>(type: "integer", nullable: false),
                    ItemClassId = table.Column<int>(type: "integer", nullable: true),
                    ItemClassName = table.Column<string>(type: "text", nullable: true),
                    ItemSubclassId = table.Column<int>(type: "integer", nullable: true),
                    ItemSubclassName = table.Column<string>(type: "text", nullable: true),
                    ProfessionId = table.Column<int>(type: "integer", nullable: true),
                    ProfessionName = table.Column<string>(type: "text", nullable: true),
                    ProfessionSkillLevel = table.Column<int>(type: "integer", nullable: true),
                    SkillDisplayString = table.Column<string>(type: "text", nullable: true),
                    CraftingReagent = table.Column<string>(type: "text", nullable: true),
                    InventoryType = table.Column<string>(type: "text", nullable: true),
                    InventoryTypeName = table.Column<string>(type: "text", nullable: true),
                    PurchasePrice = table.Column<long>(type: "bigint", nullable: true),
                    SellPrice = table.Column<long>(type: "bigint", nullable: true),
                    MaxCount = table.Column<int>(type: "integer", nullable: true),
                    IsEquippable = table.Column<bool>(type: "boolean", nullable: false),
                    IsStackable = table.Column<bool>(type: "boolean", nullable: false),
                    PurchaseQuantity = table.Column<int>(type: "integer", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    LastFetchedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemMetaData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommodityAuctionSnapshots",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IngestionRunId = table.Column<long>(type: "bigint", nullable: false),
                    FetchedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ApiEndPoint = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommodityAuctionSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommodityAuctionSnapshots_IngestionRuns_IngestionRunId",
                        column: x => x.IngestionRunId,
                        principalTable: "IngestionRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommodityAuctions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommodityAuctionSnapshotId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    UnitPrice = table.Column<long>(type: "bigint", nullable: false),
                    TimeLeft = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommodityAuctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommodityAuctions_CommodityAuctionSnapshots_CommodityAuctio~",
                        column: x => x.CommodityAuctionSnapshotId,
                        principalTable: "CommodityAuctionSnapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommodityAuctions_CommodityAuctionSnapshotId",
                table: "CommodityAuctions",
                column: "CommodityAuctionSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_CommodityAuctions_ItemId_CommodityAuctionSnapshotId",
                table: "CommodityAuctions",
                columns: new[] { "ItemId", "CommodityAuctionSnapshotId" })
                .Annotation("Npgsql:IndexInclude", new[] { "UnitPrice", "Quantity" });

            migrationBuilder.CreateIndex(
                name: "IX_CommodityAuctionSnapshots_FetchedAtUtc",
                table: "CommodityAuctionSnapshots",
                column: "FetchedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_CommodityAuctionSnapshots_IngestionRunId",
                table: "CommodityAuctionSnapshots",
                column: "IngestionRunId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommodityAuctions");

            migrationBuilder.DropTable(
                name: "ItemMetaData");

            migrationBuilder.DropTable(
                name: "CommodityAuctionSnapshots");

            migrationBuilder.DropTable(
                name: "IngestionRuns");
        }
    }
}
