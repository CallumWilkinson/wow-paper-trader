using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wow_paper_trader.Ingestor.Migrations
{
    /// <inheritdoc />
    public partial class AddIngestionRunStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "IngestionRuns",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "IngestionRuns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorStack",
                table: "IngestionRuns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedAtUtc",
                table: "IngestionRuns",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAtUtc",
                table: "IngestionRuns",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "IngestionRuns");

            migrationBuilder.DropColumn(
                name: "ErrorStack",
                table: "IngestionRuns");

            migrationBuilder.DropColumn(
                name: "FinishedAtUtc",
                table: "IngestionRuns");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAtUtc",
                table: "IngestionRuns");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "IngestionRuns",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
