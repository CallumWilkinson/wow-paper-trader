using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WowPaperTrader.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentPriceLevels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentPriceLevel_AuctionMarketSnapshots_AuctionMarketSnaps~",
                table: "CurrentPriceLevel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentPriceLevel",
                table: "CurrentPriceLevel");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CurrentPriceLevel");

            migrationBuilder.RenameTable(
                name: "CurrentPriceLevel",
                newName: "CurrentPriceLevels");

            migrationBuilder.RenameColumn(
                name: "AuctionMarketSnapshotObservedAtUtc",
                table: "CurrentPriceLevels",
                newName: "ObservedAtUtc");

            migrationBuilder.RenameColumn(
                name: "AuctionMarketSnapshotAuctionMarketId",
                table: "CurrentPriceLevels",
                newName: "AuctionMarketId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentPriceLevel_AuctionMarketSnapshotAuctionMarketId_Auct~",
                table: "CurrentPriceLevels",
                newName: "IX_CurrentPriceLevels_AuctionMarketId_ObservedAtUtc");

            migrationBuilder.AddColumn<bool>(
                name: "FilterApplied",
                table: "ItemMarketSnapshots",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "FilteredListingCount",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "FilteredMeanUnitPrice",
                table: "ItemMarketSnapshots",
                type: "numeric(28,6)",
                precision: 28,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FilteredMinimumUnitPrice",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FilteredQuantityWeightedMeanUnitPrice",
                table: "ItemMarketSnapshots",
                type: "numeric(28,6)",
                precision: 28,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FilteredTotalQuantity",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ListingCount",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "LowerFenceUnitPrice",
                table: "ItemMarketSnapshots",
                type: "numeric(28,6)",
                precision: 28,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MeanUnitPrice",
                table: "ItemMarketSnapshots",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "MedianUnitPrice",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MinimumUnitPrice",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "P10UnitPrice",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "P25UnitPrice",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "P75UnitPrice",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "P90UnitPrice",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "QuantityWeightedMeanUnitPrice",
                table: "ItemMarketSnapshots",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "TotalQuantity",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "UpperFenceUnitPrice",
                table: "ItemMarketSnapshots",
                type: "numeric(28,6)",
                precision: 28,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentPriceLevels",
                table: "CurrentPriceLevels",
                columns: new[] { "AuctionMarketId", "ItemId", "VariantKey", "UnitPrice" });

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentPriceLevels_AuctionMarketSnapshots_AuctionMarketId_O~",
                table: "CurrentPriceLevels",
                columns: new[] { "AuctionMarketId", "ObservedAtUtc" },
                principalTable: "AuctionMarketSnapshots",
                principalColumns: new[] { "AuctionMarketId", "ObservedAtUtc" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentPriceLevels_AuctionMarketSnapshots_AuctionMarketId_O~",
                table: "CurrentPriceLevels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentPriceLevels",
                table: "CurrentPriceLevels");

            migrationBuilder.DropColumn(
                name: "FilterApplied",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "FilteredListingCount",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "FilteredMeanUnitPrice",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "FilteredMinimumUnitPrice",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "FilteredQuantityWeightedMeanUnitPrice",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "FilteredTotalQuantity",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "ListingCount",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "LowerFenceUnitPrice",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "MeanUnitPrice",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "MedianUnitPrice",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "MinimumUnitPrice",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "P10UnitPrice",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "P25UnitPrice",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "P75UnitPrice",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "P90UnitPrice",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "QuantityWeightedMeanUnitPrice",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "TotalQuantity",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "UpperFenceUnitPrice",
                table: "ItemMarketSnapshots");

            migrationBuilder.RenameTable(
                name: "CurrentPriceLevels",
                newName: "CurrentPriceLevel");

            migrationBuilder.RenameColumn(
                name: "ObservedAtUtc",
                table: "CurrentPriceLevel",
                newName: "AuctionMarketSnapshotObservedAtUtc");

            migrationBuilder.RenameColumn(
                name: "AuctionMarketId",
                table: "CurrentPriceLevel",
                newName: "AuctionMarketSnapshotAuctionMarketId");

            migrationBuilder.RenameIndex(
                name: "IX_CurrentPriceLevels_AuctionMarketId_ObservedAtUtc",
                table: "CurrentPriceLevel",
                newName: "IX_CurrentPriceLevel_AuctionMarketSnapshotAuctionMarketId_Auct~");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "CurrentPriceLevel",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentPriceLevel",
                table: "CurrentPriceLevel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentPriceLevel_AuctionMarketSnapshots_AuctionMarketSnaps~",
                table: "CurrentPriceLevel",
                columns: new[] { "AuctionMarketSnapshotAuctionMarketId", "AuctionMarketSnapshotObservedAtUtc" },
                principalTable: "AuctionMarketSnapshots",
                principalColumns: new[] { "AuctionMarketId", "ObservedAtUtc" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
