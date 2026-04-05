namespace wow_paper_trader.Persistence.Tests;

using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

public sealed class ItemSearchQueryTests : IClassFixture<SqlServerTestDbFixture>
{
    private readonly SqlServerTestDbFixture _db;

    public ItemSearchQueryTests(SqlServerTestDbFixture db)
    {
        _db = db;
    }

    [Fact]
    public async Task SearchByNameAsync_ShouldReturnItems_WithNameContainingSearchTerm()
    {
        //arrange
        await using (var arrangeDbContext = await _db.CreateArrangeDbContextAsync())
        {
            var repo = new ItemMetaDataRepository(arrangeDbContext, NullLogger<ItemMetaDataRepository>.Instance);

            var metaData = ItemMetaDataRecordFactory.CreateLinenSearchTestRecords();

            await repo.SaveItemMetaDataAsync(metaData, CancellationToken.None);

        }

        const string searchString = "linen";

        //act
        List<ItemSearchResult> results;

        await using (var actDbContext = _db.CreateAssertDbContext())
        {
            var query = new ItemSearchQuery(actDbContext);

            results = await query.SearchByNameAsync(searchString, CancellationToken.None);
        }

        //assert

        var expectedResults = ItemSearchResultFactory.CreateExpectedLinenResults();

        results.Should().NotBeNull();
        results.Should().BeEquivalentTo(expectedResults);
    }
}