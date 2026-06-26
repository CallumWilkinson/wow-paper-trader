using FluentAssertions;
using WowPaperTrader.Persistence.ReadServices;
using WowPaperTrader.Persistence.Tests.TestFixtures;
using WowPaperTrader.Persistence.Tests.TestHelpers;

namespace WowPaperTrader.Persistence.Tests.IntegrationTests.QueryTests;

public sealed class ItemIdsWithoutMetadataReadServiceTests(PostgreSqlTestDbFixture db)
    : PostgreSqlIntegrationTestBase(db)
{
    [Fact]
    public async Task
        GetItemIdsWithoutMetadataAsync_ShouldReturn_AllUniqueItemIdsThatDoNotHaveMetadata_ContainedIn_CommodityAuctionsTable()
    {
        //arrange
        await using var arrangeDbContext = db.CreateDbContext();

        var snapshotTime = new DateTime(2026, 3, 12, 9, 0, 0, DateTimeKind.Utc);

        await CommodityAuctionSnapshotTestFactory.AddNewerSnapshotToDbAsync(arrangeDbContext, snapshotTime);

        var query = new ItemIdsWithoutMetadataReadService(arrangeDbContext);

        //act
        var result = await query.GetItemIdsWithoutMetadataAsync(CancellationToken.None);

        //assert
        await using var assertDbContext = db.CreateDbContext();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(2770);
        result.Should().Contain(9999);
    }
}