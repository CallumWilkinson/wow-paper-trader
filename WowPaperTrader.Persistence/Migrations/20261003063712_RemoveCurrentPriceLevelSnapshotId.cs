using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WowPaperTrader.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCurrentPriceLevelSnapshotId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuctionMarketSnapshotId",
                table: "CurrentPriceLevels");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMarketSnapshots_ObservedAtUtc",
                table: "ItemMarketSnapshots",
                column: "ObservedAtUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemMarketSnapshots_ObservedAtUtc",
                table: "ItemMarketSnapshots");

            migrationBuilder.AddColumn<long>(
                name: "AuctionMarketSnapshotId",
                table: "CurrentPriceLevels",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
