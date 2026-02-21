using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using wow_paper_trader.Ingestor;

public sealed class DataIngestorBackgroundServiceTests : IClassFixture<SqliteInMemoryDbFixture>
{
    private readonly SqliteInMemoryDbFixture _db;

    public DataIngestorBackgroundServiceTests(SqliteInMemoryDbFixture db)
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

        await using var dbContext = _db.CreateDbContext();

        var testBuilder = new TestAuctionSnapshotIngestionRunBuilder(authJson, auctionsJson);
        var ingestionRunOrchestrator = testBuilder.CreateOrchestrator(dbContext);

        //Act   
        await ingestionRunOrchestrator.RunOnceAsync(CancellationToken.None);

        //Assert
        //use 1 assertdB per test (not per assertion)
        await using var assertDb = _db.CreateDbContext();

        //assert on the sql in memory db
    }
}