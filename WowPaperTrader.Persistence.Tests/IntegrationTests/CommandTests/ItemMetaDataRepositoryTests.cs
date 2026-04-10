using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using WowPaperTrader.Persistence.EntityMappers;
using WowPaperTrader.Persistence.Repositories;
using WowPaperTrader.Persistence.Tests.TestFixtures;
using WowPaperTrader.Persistence.Tests.TestHelpers;

namespace WowPaperTrader.Persistence.Tests.IntegrationTests.CommandTests;

public sealed class ItemMetaDataRepositoryTests : IClassFixture<SqliteInMemoryDbFixture>
{
    private readonly SqliteInMemoryDbFixture _db;

    public ItemMetaDataRepositoryTests(SqliteInMemoryDbFixture db)
    {
        _db = db;
    }

    [Fact]
    public async Task SaveItemMetaDataAsync_Should_Save_AllPersistedFields()
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

        var actual = await assertDbContext.ItemMetaData
            .AsNoTracking()
            .OrderBy(x => x.ItemId)
            .ToListAsync();

        var expected = listOfRecords
            .Select(ItemMetaDataMapper.MapToEntity)
            .OrderBy(x => x.ItemId)
            .ToList();

        actual.Should().BeEquivalentTo(expected, options => options
            .Excluding(x => x.Id)
            .WithStrictOrdering());
    }
}