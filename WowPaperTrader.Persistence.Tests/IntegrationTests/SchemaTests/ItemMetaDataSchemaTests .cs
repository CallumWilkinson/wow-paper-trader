using System.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Persistence.Tests.TestFixtures;

namespace WowPaperTrader.Persistence.Tests.IntegrationTests.SchemaTests;

public sealed class ItemMetaDataSchemaTests : IClassFixture<SqliteInMemoryDbFixture>
{
    private readonly SqliteInMemoryDbFixture _db;

    public ItemMetaDataSchemaTests(SqliteInMemoryDbFixture db)
    {
        _db = db;
    }

    [Fact]
    public async Task ItemMetaData_Table_Should_Have_Expected_Columns()
    {
        await using var dbContext = await _db.CreateArrangeDbContextAsync();

        var actualColumns = await GetTableColumnNamesAsync(dbContext, "ItemMetaData");
        var expectedColumns = GetExpectedItemMetaDataColumnNames();

        actualColumns.Should().Equal(expectedColumns);
    }

    private static async Task<List<string>> GetTableColumnNamesAsync(ApplicationDbContext dbContext, string tableName)
    {
        var connection = dbContext.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open) await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = $"PRAGMA table_info({tableName})";

        await using var reader = await command.ExecuteReaderAsync();

        var columnNames = new List<string>();

        while (await reader.ReadAsync())
        {
            var columnName = reader.GetString(reader.GetOrdinal("name"));
            columnNames.Add(columnName);
        }

        return columnNames;
    }

    private static List<string> GetExpectedItemMetaDataColumnNames()
    {
        return new List<string>
        {
            "Id",
            "ItemId",
            "Name",
            "QualityType",
            "QualityName",
            "Level",
            "RequiredLevel",
            "ItemClassId",
            "ItemClassName",
            "ItemSubclassId",
            "ItemSubclassName",
            "ProfessionId",
            "ProfessionName",
            "ProfessionSkillLevel",
            "SkillDisplayString",
            "CraftingReagent",
            "InventoryType",
            "InventoryTypeName",
            "PurchasePrice",
            "SellPrice",
            "MaxCount",
            "IsEquippable",
            "IsStackable",
            "PurchaseQuantity",
            "ImageUrl",
            "LastFetchedUtc"
        };
    }
}