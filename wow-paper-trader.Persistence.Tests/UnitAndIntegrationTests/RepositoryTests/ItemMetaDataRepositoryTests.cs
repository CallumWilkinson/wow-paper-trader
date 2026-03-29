using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

public sealed class ItemMetaDataRepositoryTests : IClassFixture<SqliteInMemoryDbFixture>
{
    private readonly SqliteInMemoryDbFixture _db;

    public ItemMetaDataRepositoryTests(SqliteInMemoryDbFixture db)
    {
        _db = db;
    }

    [Fact]
    public async Task SaveItemMetaDataAsync_Should_CorrectlySaveAllFields_ToAnEmptyTestDb_UsingAListOfFakeItemMetaDataRecords()
    {
        //arrange
        await using var arrangeDbContext = await _db.CreateArrangeDbContextAsync();

        var logger = NullLogger<ItemMetaDataRepository>.Instance;

        var repo = new ItemMetaDataRepository(arrangeDbContext, logger);

        var listOfRecords = ItemMetaDataRecordFactory.CreateRecordsList();

        //act
        await repo.SaveItemMetaDataAsync(listOfRecords, CancellationToken.None);

        //assert
        await using var assertDbContext = _db.CreateAssertDbContext();

        var actual = await assertDbContext.ItemMetaData.OrderBy(x => x.ItemId).Select(x => new
        {
            x.ItemId,
            x.Name,
            x.QualityType,
            x.QualityName,
            x.Level,
            x.RequiredLevel,
            x.ItemClassId,
            x.ItemClassName,
            x.ItemSubclassId,
            x.ItemSubclassName,
            x.ProfessionId,
            x.ProfessionName,
            x.ProfessionSkillLevel,
            x.SkillDisplayString,
            x.CraftingReagent,
            x.InventoryType,
            x.InventoryTypeName,
            x.PurchasePrice,
            x.SellPrice,
            x.MaxCount,
            x.IsEquippable,
            x.IsStackable,
            x.PurchaseQuantity,
            x.LastFetchedUtc
        }).ToListAsync();

        var expected = listOfRecords.OrderBy(x => x.ItemId).Select(x => new
        {
            x.ItemId,
            x.Name,
            x.QualityType,
            x.QualityName,
            x.Level,
            x.RequiredLevel,
            x.ItemClassId,
            x.ItemClassName,
            x.ItemSubclassId,
            x.ItemSubclassName,
            x.ProfessionId,
            x.ProfessionName,
            x.ProfessionSkillLevel,
            x.SkillDisplayString,
            x.CraftingReagent,
            x.InventoryType,
            x.InventoryTypeName,
            x.PurchasePrice,
            x.SellPrice,
            x.MaxCount,
            x.IsEquippable,
            x.IsStackable,
            x.PurchaseQuantity,
            x.LastFetchedUtc
        }).ToList();

        actual.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

}