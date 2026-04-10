using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using WowPaperTrader.Persistence.Repositories;
using WowPaperTrader.Persistence.Tests.TestFixtures;
using WowPaperTrader.Persistence.Tests.TestHelpers;

namespace WowPaperTrader.Persistence.Tests.IntegrationTests.CommandTests;

public sealed class CommodityAuctionRepositoryTests : IClassFixture<SqliteInMemoryDbFixture>
{
    private readonly SqliteInMemoryDbFixture _db;

    public CommodityAuctionRepositoryTests(SqliteInMemoryDbFixture db)
    {
        _db = db;
    }

    [Fact]
    public async Task SaveSnapshotAsync_ShouldSaveAuctionRows_ToATestDatabase_UsingFakeApiResult()
    {
        //arrange
        await using var arrangeDbContext = await _db.CreateArrangeDbContextAsync();

        var logger = NullLogger<CommodityAuctionRepository>.Instance;

        var repo = new CommodityAuctionRepository(arrangeDbContext, logger);

        var run = await repo.CreateIngestionRunAsync(CancellationToken.None);

        var result = WowApiResultFactory.Create();

        //act
        await repo.SaveSnapshotAsync(run, result, CancellationToken.None);

        //assert
        await using var assertDbContext = _db.CreateAssertDbContext();

        var savedIngestionRuns = await assertDbContext.IngestionRuns.ToListAsync();
        savedIngestionRuns.Should().ContainSingle();

        var savedSnapshots = await assertDbContext.CommodityAuctionSnapshots.ToListAsync();
        savedSnapshots.Should().ContainSingle();

        var singleSavedSnapshot = savedSnapshots.Single();
        singleSavedSnapshot.IngestionRunId.Should().Be(run.Id);

        //use Select() to avoid testing EF navigation properties
        var savedAuctionRows = await assertDbContext.CommodityAuctions
            .Select(a => new
            {
                a.CommodityAuctionSnapshotId,
                a.ItemId,
                a.Quantity,
                a.UnitPrice,
                a.TimeLeft
            })
            .ToListAsync();

        var expectedAuctionRows = result.Payload.Auctions
            .Select(a => new
            {
                CommodityAuctionSnapshotId = singleSavedSnapshot.Id,
                a.ItemId,
                a.Quantity,
                a.UnitPrice,
                a.TimeLeft
            })
            .ToList();

        savedAuctionRows.Should().NotBeNull();
        savedAuctionRows.Should().BeEquivalentTo(expectedAuctionRows);
    }
}