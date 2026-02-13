using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wow_paper_trader.Ingestor.Migrations
{
    /// <inheritdoc />
    public partial class FixCommodityAuctionsQuantitySpelling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quanity",
                table: "CommodityAuctions",
                newName: "Quantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "CommodityAuctions",
                newName: "Quanity");
        }
    }
}
