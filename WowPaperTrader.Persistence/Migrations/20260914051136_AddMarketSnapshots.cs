using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WowPaperTrader.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMarketSnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionMarketSnapshot_AuctionMarkets_AuctionMarketId",
                table: "AuctionMarketSnapshot");

            migrationBuilder.DropForeignKey(
                name: "FK_AuctionMarketSnapshot_IngestionRuns_IngestionRunId",
                table: "AuctionMarketSnapshot");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrentPriceLevel_AuctionMarketSnapshot_AuctionMarketSnapsh~",
                table: "CurrentPriceLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemMarketSnapshot_AuctionMarketSnapshot_AuctionMarketSnaps~",
                table: "ItemMarketSnapshot");

            migrationBuilder.DropIndex(
                name: "IX_CurrentPriceLevel_AuctionMarketSnapshotId",
                table: "CurrentPriceLevel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemMarketSnapshot",
                table: "ItemMarketSnapshot");

            migrationBuilder.DropIndex(
                name: "IX_ItemMarketSnapshot_AuctionMarketSnapshotId",
                table: "ItemMarketSnapshot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuctionMarketSnapshot",
                table: "AuctionMarketSnapshot");

            migrationBuilder.DropIndex(
                name: "IX_AuctionMarketSnapshot_AuctionMarketId",
                table: "AuctionMarketSnapshot");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ItemMarketSnapshot");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AuctionMarketSnapshot");

            migrationBuilder.RenameTable(
                name: "ItemMarketSnapshot",
                newName: "ItemMarketSnapshots");

            migrationBuilder.RenameTable(
                name: "AuctionMarketSnapshot",
                newName: "AuctionMarketSnapshots");

            migrationBuilder.RenameColumn(
                name: "AuctionMarketSnapshotId",
                table: "ItemMarketSnapshots",
                newName: "AuctionMarketId");

            migrationBuilder.RenameIndex(
                name: "IX_AuctionMarketSnapshot_IngestionRunId",
                table: "AuctionMarketSnapshots",
                newName: "IX_AuctionMarketSnapshots_IngestionRunId");

            migrationBuilder.AddColumn<long>(
                name: "AuctionMarketSnapshotAuctionMarketId",
                table: "CurrentPriceLevel",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "AuctionMarketSnapshotObservedAtUtc",
                table: "CurrentPriceLevel",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ObservedAtUtc",
                table: "ItemMarketSnapshots",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ObservedAtUtc",
                table: "AuctionMarketSnapshots",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemMarketSnapshots",
                table: "ItemMarketSnapshots",
                columns: new[] { "AuctionMarketId", "ItemId", "VariantKey", "ObservedAtUtc" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuctionMarketSnapshots",
                table: "AuctionMarketSnapshots",
                columns: new[] { "AuctionMarketId", "ObservedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_CurrentPriceLevel_AuctionMarketSnapshotAuctionMarketId_Auct~",
                table: "CurrentPriceLevel",
                columns: new[] { "AuctionMarketSnapshotAuctionMarketId", "AuctionMarketSnapshotObservedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemMarketSnapshots_AuctionMarketId_ObservedAtUtc",
                table: "ItemMarketSnapshots",
                columns: new[] { "AuctionMarketId", "ObservedAtUtc" });

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionMarketSnapshots_AuctionMarkets_AuctionMarketId",
                table: "AuctionMarketSnapshots",
                column: "AuctionMarketId",
                principalTable: "AuctionMarkets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionMarketSnapshots_IngestionRuns_IngestionRunId",
                table: "AuctionMarketSnapshots",
                column: "IngestionRunId",
                principalTable: "IngestionRuns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentPriceLevel_AuctionMarketSnapshots_AuctionMarketSnaps~",
                table: "CurrentPriceLevel",
                columns: new[] { "AuctionMarketSnapshotAuctionMarketId", "AuctionMarketSnapshotObservedAtUtc" },
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionMarketSnapshots_AuctionMarkets_AuctionMarketId",
                table: "AuctionMarketSnapshots");

            migrationBuilder.DropForeignKey(
                name: "FK_AuctionMarketSnapshots_IngestionRuns_IngestionRunId",
                table: "AuctionMarketSnapshots");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrentPriceLevel_AuctionMarketSnapshots_AuctionMarketSnaps~",
                table: "CurrentPriceLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemMarketSnapshots_AuctionMarketSnapshots_AuctionMarketId_~",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropIndex(
                name: "IX_CurrentPriceLevel_AuctionMarketSnapshotAuctionMarketId_Auct~",
                table: "CurrentPriceLevel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemMarketSnapshots",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropIndex(
                name: "IX_ItemMarketSnapshots_AuctionMarketId_ObservedAtUtc",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuctionMarketSnapshots",
                table: "AuctionMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "AuctionMarketSnapshotAuctionMarketId",
                table: "CurrentPriceLevel");

            migrationBuilder.DropColumn(
                name: "AuctionMarketSnapshotObservedAtUtc",
                table: "CurrentPriceLevel");

            migrationBuilder.DropColumn(
                name: "ObservedAtUtc",
                table: "ItemMarketSnapshots");

            migrationBuilder.DropColumn(
                name: "ObservedAtUtc",
                table: "AuctionMarketSnapshots");

            migrationBuilder.RenameTable(
                name: "ItemMarketSnapshots",
                newName: "ItemMarketSnapshot");

            migrationBuilder.RenameTable(
                name: "AuctionMarketSnapshots",
                newName: "AuctionMarketSnapshot");

            migrationBuilder.RenameColumn(
                name: "AuctionMarketId",
                table: "ItemMarketSnapshot",
                newName: "AuctionMarketSnapshotId");

            migrationBuilder.RenameIndex(
                name: "IX_AuctionMarketSnapshots_IngestionRunId",
                table: "AuctionMarketSnapshot",
                newName: "IX_AuctionMarketSnapshot_IngestionRunId");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "ItemMarketSnapshot",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "AuctionMarketSnapshot",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemMarketSnapshot",
                table: "ItemMarketSnapshot",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuctionMarketSnapshot",
                table: "AuctionMarketSnapshot",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentPriceLevel_AuctionMarketSnapshotId",
                table: "CurrentPriceLevel",
                column: "AuctionMarketSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMarketSnapshot_AuctionMarketSnapshotId",
                table: "ItemMarketSnapshot",
                column: "AuctionMarketSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionMarketSnapshot_AuctionMarketId",
                table: "AuctionMarketSnapshot",
                column: "AuctionMarketId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionMarketSnapshot_AuctionMarkets_AuctionMarketId",
                table: "AuctionMarketSnapshot",
                column: "AuctionMarketId",
                principalTable: "AuctionMarkets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionMarketSnapshot_IngestionRuns_IngestionRunId",
                table: "AuctionMarketSnapshot",
                column: "IngestionRunId",
                principalTable: "IngestionRuns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentPriceLevel_AuctionMarketSnapshot_AuctionMarketSnapsh~",
                table: "CurrentPriceLevel",
                column: "AuctionMarketSnapshotId",
                principalTable: "AuctionMarketSnapshot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMarketSnapshot_AuctionMarketSnapshot_AuctionMarketSnaps~",
                table: "ItemMarketSnapshot",
                column: "AuctionMarketSnapshotId",
                principalTable: "AuctionMarketSnapshot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
