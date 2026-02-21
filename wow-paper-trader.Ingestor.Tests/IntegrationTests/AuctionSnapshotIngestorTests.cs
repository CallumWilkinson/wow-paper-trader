using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using wow_paper_trader.Ingestor;

public sealed class AuctionSnapshotIngestorTests : IClassFixture<SqliteInMemoryDbFixture>
{
    private readonly SqliteInMemoryDbFixture _db;

    public AuctionSnapshotIngestorTests(SqliteInMemoryDbFixture db)
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

        //EXTRACT TO SETUP FUNCTION
        //START

        IConfiguration authConfig = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Blizzard:ClientId"] = "test-client-id",
            ["Blizzard:ClientSecret"] = "test-client-secret",
        })
        .Build();

        var stubHandlerWithAuthJson = new StubHttpMessageHandler(authJson, System.Net.HttpStatusCode.OK);

        var httpClientWithAuthStub = new HttpClient(stubHandlerWithAuthJson);

        var battleNetAuthClient = new BattleNetAuthClient(authConfig, httpClientWithAuthStub);


        var stubHandlerWithAuctionsJson = new StubHttpMessageHandler(auctionsJson, System.Net.HttpStatusCode.OK);

        var httpClientWithAuctionsStub = new HttpClient(stubHandlerWithAuctionsJson);

        var wowApiClient = new WowApiClient(httpClientWithAuctionsStub);


        ILogger<AuctionSnapshotIngestionRunOrchestrator> logger = NullLogger<AuctionSnapshotIngestionRunOrchestrator>.Instance;

        IngestorDbContext dbContext = _db.CreateDbContext();

        var ingestionRunOrchestrator = new AuctionSnapshotIngestionRunOrchestrator(logger, dbContext, battleNetAuthClient, wowApiClient);

        //END SETUP FUNCTION

        //Act   

        await ingestionRunOrchestrator.RunOnceAsync(CancellationToken.None);

        //Assert

        //assert on the sql in memory db
    }
}