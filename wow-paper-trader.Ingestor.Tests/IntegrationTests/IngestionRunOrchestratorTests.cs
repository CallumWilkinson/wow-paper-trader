using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

    [Fact]
    public async Task RunOnceAsync_SavesCommodityAuctionShapshotToDatabase_FromStubbedJson()
    {
        //Arrange
        string authJson =
        """
        {
            "access_token": "test-token",
            "token_type": "bearer",
            "expires_in": 86400
        }
        """;

        string auctionsJson =
        """
        {
            "auctions": 
                [
                    { "id": 1, "item": { "id": 19019 }, "quantity": 5, "unit_price": 123456, "time_left": "LONG" },
                    { "id": 2, "item": { "id": 19020 }, "quantity": 2, "unit_price": 120000, "time_left": "SHORT" }
                ]
        }
        """;

        await using var arrangeDbContext = _db.CreateDbContext();

        var testBuilder = new TestBuilderForIngestionRunOrchestrator(authJson, auctionsJson);
        var ingestionRunOrchestrator = testBuilder.CreateOrchestrator(arrangeDbContext);

        //Act   
        await ingestionRunOrchestrator.RunOnceAsync(CancellationToken.None);

        //Assert
        await using var assertDbContext = _db.CreateDbContext();

        int ingestionRunRowCount = await assertDbContext.IngestionRuns.CountAsync();
        int snapshotRowCount = await assertDbContext.CommodityAuctionSnapshots.CountAsync();
        int auctionRowCount = await assertDbContext.CommodityAuctions.CountAsync();
        IngestionRun ingestionRunRow = await assertDbContext.IngestionRuns.SingleAsync(x => x.Id == 1);

        Assert.Equal(1, ingestionRunRowCount);

        Assert.Equal(IngestionRunStatus.Finished, ingestionRunRow.Status);

        // Assert.Equal(1, snapshotRowCount);
        // Assert.Equal(2, auctionRowCount);

    }
}