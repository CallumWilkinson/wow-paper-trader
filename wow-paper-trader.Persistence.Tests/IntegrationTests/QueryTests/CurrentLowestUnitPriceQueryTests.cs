using FluentAssertions;

public sealed class CurrentLowestUnitPriceQueryTests : IClassFixture<SqliteInMemoryDbFixture>
{
    private readonly SqliteInMemoryDbFixture _db;

    public CurrentLowestUnitPriceQueryTests(SqliteInMemoryDbFixture db)
    {
        _db = db;
    }

    [Fact]
    public async Task GetAsync_ShouldReturn_LowestUnitPrice_FromLatestSnapshot()
    {
        //arrange
        await using var arrangeDbContext = await _db.CreateArrangeDbContextAsync();

        var query = new CurrentLowestUnitPriceQuery(arrangeDbContext);

        const long copperOreItemId = 2770;

        var olderTime = new DateTime(2026, 3, 12, 8, 0, 0, DateTimeKind.Utc);
        await CommodityAuctionSnapshotTestFactory.AddOlderSnapshotToDbAsync(arrangeDbContext, olderTime);

        var newerTime = new DateTime(2026, 3, 12, 9, 0, 0, DateTimeKind.Utc);
        await CommodityAuctionSnapshotTestFactory.AddNewerSnapshotToDbAsync(arrangeDbContext, newerTime);


        //act
        var result = await query.GetAsync(copperOreItemId, CancellationToken.None);

        //assert
        await using var assertDbContext = _db.CreateAssertDbContext();

        result.Should().NotBeNull();
        result.ItemId.Should().Be(copperOreItemId);
        result.UnitPrice.Should().Be(80);
        result.PriceTakenAtUtc.Should().Be(newerTime);
    }
}
