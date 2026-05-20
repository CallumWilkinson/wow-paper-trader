using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WowPaperTrader.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceHistoryReadIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CommodityAuctionSnapshots_FetchedAtUtc",
                table: "CommodityAuctionSnapshots",
                column: "FetchedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_CommodityAuctions_ItemId_CommodityAuctionSnapshotId",
                table: "CommodityAuctions",
                columns: new[] { "ItemId", "CommodityAuctionSnapshotId" })
                .Annotation("SqlServer:Include", new[] { "UnitPrice", "Quantity" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CommodityAuctionSnapshots_FetchedAtUtc",
                table: "CommodityAuctionSnapshots");

            migrationBuilder.DropIndex(
                name: "IX_CommodityAuctions_ItemId_CommodityAuctionSnapshotId",
                table: "CommodityAuctions");
        }
    }
}
