namespace wow_paper_trader.Persistence.Tests;

using FluentAssertions;

public sealed class CommodityAuctionItemIdQueryTests : IClassFixture<SqliteInMemoryDbFixture>
{
    private readonly SqliteInMemoryDbFixture _db;

    public CommodityAuctionItemIdQueryTests(SqliteInMemoryDbFixture db)
    {
        _db = db;
    }

    [Fact]
    public async Task GetAllUniqueItemIdsAsync_ShouldReturn_AllUniqueItemIds_ContainedIn_CommodityAuctionsTable()
    {
        //arrange
        await using var arrangeDbContext = await _db.CreateArrangeDbContextAsync();

        var snapshotTime = new DateTime(2026, 3, 12, 9, 0, 0, DateTimeKind.Utc);

        await CommodityAuctionSnapshotTestFactory.AddNewerSnapshotToDbAsync(arrangeDbContext, snapshotTime);

        var query = new CommodityAuctionItemIdQuery(arrangeDbContext);

        //act
        List<long> result = await query.GetAllUniqueItemIdsAsync(CancellationToken.None);

        //assert
        await using var assertDbContext = _db.CreateAssertDbContext();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(2770);
        result.Should().Contain(9999);
    }
}