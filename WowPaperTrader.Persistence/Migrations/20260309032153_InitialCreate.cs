using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WowPaperTrader.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IngestionRuns",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorStack = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngestionRuns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommodityAuctionSnapshots",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IngestionRunId = table.Column<long>(type: "bigint", nullable: false),
                    FetchedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApiEndPoint = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommodityAuctionSnapshotId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    UnitPrice = table.Column<long>(type: "bigint", nullable: false),
                    TimeLeft = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommodityAuctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommodityAuctions_CommodityAuctionSnapshots_CommodityAuctionSnapshotId",
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
                name: "CommodityAuctionSnapshots");

            migrationBuilder.DropTable(
                name: "IngestionRuns");
        }
    }
}
