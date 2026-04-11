using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WowPaperTrader.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddItemMetaDataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemMetaDataEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QualityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QualityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    RequiredLevel = table.Column<int>(type: "int", nullable: false),
                    ItemClassId = table.Column<int>(type: "int", nullable: true),
                    ItemClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemSubclassId = table.Column<int>(type: "int", nullable: true),
                    ItemSubclassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessionId = table.Column<int>(type: "int", nullable: true),
                    ProfessionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessionSkillLevel = table.Column<int>(type: "int", nullable: true),
                    SkillDisplayString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CraftingReagent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InventoryType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InventoryTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchasePrice = table.Column<long>(type: "bigint", nullable: true),
                    SellPrice = table.Column<long>(type: "bigint", nullable: true),
                    MaxCount = table.Column<int>(type: "int", nullable: true),
                    IsEquippable = table.Column<bool>(type: "bit", nullable: false),
                    IsStackable = table.Column<bool>(type: "bit", nullable: false),
                    PurchaseQuantity = table.Column<int>(type: "int", nullable: true),
                    LastFetchedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemMetaData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemMetaDataEntity");
        }
    }
}
