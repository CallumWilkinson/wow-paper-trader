using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using WowPaperTrader.Application.Features.Read.ItemSearch;
using WowPaperTrader.Persistence.ReadServices;
using WowPaperTrader.Persistence.Repositories;
using WowPaperTrader.Persistence.Tests.TestFixtures;
using WowPaperTrader.Persistence.Tests.TestHelpers;

namespace WowPaperTrader.Persistence.Tests.IntegrationTests.QueryTests;

public sealed class ItemSearchReadServiceTests(PostgreSqlTestDbFixture db) : PostgreSqlIntegrationTestBase(db)
{
    
    [Fact]
    public async Task SearchByNameAsync_ShouldReturnItems_WithNameContainingSearchTerm()
    {
        //arrange
        await using (var arrangeDbContext = db.CreateDbContext())
        {
            var repo = new ItemMetadataRepository(arrangeDbContext, NullLogger<ItemMetadataRepository>.Instance);

            var metaData = ItemMetaDataRecordFactory.CreateLinenSearchTestRecords();

            await repo.SaveItemMetaDataAsync(metaData, CancellationToken.None);
        }

        const string searchString = "linen";

        //act
        List<ItemSearchResponse> results;

        await using (var actDbContext = db.CreateDbContext())
        {
            var query = new ItemSearchReadService(actDbContext);

            results = await query.SearchByNameAsync(searchString, CancellationToken.None);
        }

        //assert

        var expectedResults = ItemSearchResultFactory.CreateExpectedLinenResults();

        results.Should().NotBeNull();
        results.Should().BeEquivalentTo(expectedResults);
    }


}