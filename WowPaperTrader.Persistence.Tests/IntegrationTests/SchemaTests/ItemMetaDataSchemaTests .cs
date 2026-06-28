using System.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Persistence.Tests.TestFixtures;
using WowPaperTrader.Persistence.Tests.TestHelpers;

namespace WowPaperTrader.Persistence.Tests.IntegrationTests.SchemaTests;

public sealed class ItemMetaDataSchemaTests(PostgreSqlTestDbFixture db) : PostgreSqlIntegrationTestBase(db)
{
    [Fact]
    public async Task ItemMetaData_Table_Should_Have_Expected_Columns()
    {
        await using var dbContext = db.CreateDbContext();

        var actualColumns = await GetTableColumnNamesAsync(
            dbContext,
            schemaName: "public",
            tableName: "ItemMetaData");
        
        var expectedColumns = GetExpectedItemMetaDataColumnNames();

        actualColumns.Should().Equal(expectedColumns);
    }

    private static async Task<List<string>> GetTableColumnNamesAsync(
        ApplicationDbContext dbContext,
        string schemaName,
        string tableName)
    {
        var connection = dbContext.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open) await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        
        command.CommandText =  """
                               SELECT column_name
                               FROM information_schema.columns
                               WHERE table_schema = @schemaName
                                 AND table_name = @tableName
                               ORDER BY ordinal_position;
                               """;
        
        var schemaParameter = command.CreateParameter();
        schemaParameter.ParameterName = "schemaName";
        schemaParameter.Value = schemaName;
        command.Parameters.Add(schemaParameter);

        var tableParameter = command.CreateParameter();
        tableParameter.ParameterName = "tableName";
        tableParameter.Value = tableName;
        command.Parameters.Add(tableParameter);

        await using var reader = await command.ExecuteReaderAsync();

        var columnNames = new List<string>();

        while (await reader.ReadAsync())
        {
            var columnName = reader.GetString(0);
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