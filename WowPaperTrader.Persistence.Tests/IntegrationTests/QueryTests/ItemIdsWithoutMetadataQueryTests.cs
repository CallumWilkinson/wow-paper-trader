using FluentAssertions;
using WowPaperTrader.Persistence.Queries;
using WowPaperTrader.Persistence.Tests.TestFixtures;
using WowPaperTrader.Persistence.Tests.TestHelpers;

namespace WowPaperTrader.Persistence.Tests.IntegrationTests.QueryTests;

public sealed class ItemIdsWithoutMetadataQueryTests : IClassFixture<SqliteInMemoryDbFixture>
{
    private readonly SqliteInMemoryDbFixture _db;

    public ItemIdsWithoutMetadataQueryTests(SqliteInMemoryDbFixture db)
    {
        _db = db;
    }

    [Fact]
    public async Task
        GetItemIdsWithoutMetadataAsync_ShouldReturn_AllUniqueItemIdsThatDoNotHaveMetadata_ContainedIn_CommodityAuctionsTable()
    {
        //arrange
        await using var arrangeDbContext = await _db.CreateArrangeDbContextAsync();

        var snapshotTime = new DateTime(2026, 3, 12, 9, 0, 0, DateTimeKind.Utc);

        await CommodityAuctionSnapshotTestFactory.AddNewerSnapshotToDbAsync(arrangeDbContext, snapshotTime);

        var query = new ItemIdsWithoutMetadataQuery(arrangeDbContext);

        //act
        var result = await query.GetItemIdsWithoutMetadataAsync(CancellationToken.None);

        //assert
        await using var assertDbContext = _db.CreateAssertDbContext();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(2770);
        result.Should().Contain(9999);
    }
}