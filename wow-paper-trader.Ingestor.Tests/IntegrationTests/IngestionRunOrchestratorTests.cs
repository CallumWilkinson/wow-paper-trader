using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using wow_paper_trader.Ingestor;

namespace wow_paper_trader.Ingestor.Tests;


public sealed class IngestionRunOrchestratorTests : IClassFixture<SqliteInMemoryDbFixture>
{
    private readonly SqliteInMemoryDbFixture _db;

    public IngestionRunOrchestratorTests(SqliteInMemoryDbFixture db)
    {
        _db = db;
    }

    public string authJson =
        """
        {
            "access_token": "test-token",
            "token_type": "bearer",
            "expires_in": 86400
        }
        """;

    public string auctionsJson =
    """
        {
            "auctions": 
                [
                    { "id": 1, "item": { "id": 19019 }, "quantity": 5, "unit_price": 123456, "time_left": "LONG" },
                    { "id": 2, "item": { "id": 19020 }, "quantity": 2, "unit_price": 120000, "time_left": "SHORT" }
                ]
        }
        """;

    [Fact]
    public async Task RunOnceAsync_SavesCommodityAuctionShapshotToDatabase_FromStubbedJson()
    {
        //Arrange
        await using var arrangeDbContext = await _db.CreateArrangeDbContextAsync();

        var testBuilder = new TestBuilderForIngestionRunOrchestrator(authJson, auctionsJson);
        var ingestionRunOrchestrator = testBuilder.CreateOrchestrator(arrangeDbContext);

        //Act   
        await ingestionRunOrchestrator.RunOnceAsync(CancellationToken.None);

        //Assert
        await using var assertDbContext = _db.CreateAssertDbContext();

        int ingestionRunRowCount = await assertDbContext.IngestionRuns.CountAsync();
        int auctionSnapshotRowCount = await assertDbContext.CommodityAuctionSnapshots.CountAsync();
        int commoditAuctionRowCount = await assertDbContext.CommodityAuctions.CountAsync();
        var ingestionRunRow = await assertDbContext.IngestionRuns.SingleAsync();
        var auctionSnapshotRow = await assertDbContext.CommodityAuctionSnapshots.SingleAsync();
        var commodityAuctionRow1 = await assertDbContext.CommodityAuctions.SingleAsync(x => x.ItemId == 19019);

        Assert.Equal(1, ingestionRunRowCount);
        Assert.Equal(IngestionRunStatus.Finished, ingestionRunRow.Status);

        Assert.Equal(1, auctionSnapshotRowCount);
        Assert.Equal(ingestionRunRow.Id, auctionSnapshotRow.IngestionRunId);
        Assert.Equal("https://example.test/auctions/commodities?namespace=dynamic-us&locale=en_US", auctionSnapshotRow.ApiEndPoint);


        Assert.Equal(2, commoditAuctionRowCount);
        //assert correct foreign key
        Assert.Equal(auctionSnapshotRow.Id, commodityAuctionRow1.CommodityAuctionSnapshotId);
        Assert.Equal(19019, commodityAuctionRow1.ItemId);
        Assert.Equal(5, commodityAuctionRow1.Quantity);
        Assert.Equal(123456, commodityAuctionRow1.UnitPrice);
        Assert.Equal("LONG", commodityAuctionRow1.TimeLeft);

    }
}
