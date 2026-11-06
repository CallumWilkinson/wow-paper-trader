using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WowPaperTrader.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateItemMarketSnapshotNullability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UpperFenceUnitPrice",
                table: "ItemMarketSnapshots",
                type: "numeric(28,6)",
                precision: 28,
                scale: 6,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(28,6)",
                oldPrecision: 28,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "QuantityWeightedMeanUnitPrice",
                table: "ItemMarketSnapshots",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<long>(
                name: "P90UnitPrice",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "P75UnitPrice",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "P25UnitPrice",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "P10UnitPrice",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "LowerFenceUnitPrice",
                table: "ItemMarketSnapshots",
                type: "numeric(28,6)",
                precision: 28,
                scale: 6,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(28,6)",
                oldPrecision: 28,
                oldScale: 6);

            migrationBuilder.AlterColumn<long>(
                name: "FilteredTotalQuantity",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "FilteredListingCount",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "FilterApplied",
                table: "ItemMarketSnapshots",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UpperFenceUnitPrice",
                table: "ItemMarketSnapshots",
                type: "numeric(28,6)",
                precision: 28,
                scale: 6,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(28,6)",
                oldPrecision: 28,
                oldScale: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "QuantityWeightedMeanUnitPrice",
                table: "ItemMarketSnapshots",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "P90UnitPrice",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "P75UnitPrice",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "P25UnitPrice",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "P10UnitPrice",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LowerFenceUnitPrice",
                table: "ItemMarketSnapshots",
                type: "numeric(28,6)",
                precision: 28,
                scale: 6,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(28,6)",
                oldPrecision: 28,
                oldScale: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "FilteredTotalQuantity",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "FilteredListingCount",
                table: "ItemMarketSnapshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "FilterApplied",
                table: "ItemMarketSnapshots",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);
        }
    }
}
