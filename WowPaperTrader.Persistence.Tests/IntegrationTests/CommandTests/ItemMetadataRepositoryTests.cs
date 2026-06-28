using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using WowPaperTrader.Persistence.EntityMappers;
using WowPaperTrader.Persistence.Repositories;
using WowPaperTrader.Persistence.Tests.TestFixtures;
using WowPaperTrader.Persistence.Tests.TestHelpers;

namespace WowPaperTrader.Persistence.Tests.IntegrationTests.CommandTests;

public sealed class ItemMetadataRepositoryTests(PostgreSqlTestDbFixture db) : PostgreSqlIntegrationTestBase(db)
{
    [Fact]
    public async Task SaveItemMetaDataAsync_Should_Save_AllPersistedFields()
    {
        //arrange
        await using var arrangeDbContext = db.CreateDbContext();

        var logger = NullLogger<ItemMetadataRepository>.Instance;

        var repo = new ItemMetadataRepository(arrangeDbContext, logger);

        var listOfRecords = ItemMetaDataRecordFactory.CreateRecordsList();

        //act
        await repo.SaveItemMetaDataAsync(listOfRecords, CancellationToken.None);

        //assert
        await using var assertDbContext = db.CreateDbContext();

        var actual = await assertDbContext.ItemMetaData
            .AsNoTracking()
            .OrderBy(x => x.ItemId)
            .ToListAsync();

        var expected = listOfRecords
            .Select(ItemMetadataMapper.MapToEntity)
            .OrderBy(x => x.ItemId)
            .ToList();

        actual.Should().BeEquivalentTo(
            expected,
            options => options
                .Excluding(record => record.Id)
                .WithStrictOrdering()
                .Using<DateTime>(context =>
                    context.Subject.Should().BeCloseTo(
                        context.Expectation,
                        TimeSpan.FromMicroseconds(1)))
                .When(info => info.Path.EndsWith("LastFetchedUtc")));
    }
}