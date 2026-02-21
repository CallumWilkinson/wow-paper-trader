using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

public sealed class TestAuctionSnapshotIngestionRunOrchestrator
{
    private readonly string _authJson;
    private readonly string _auctionsJson;
    private readonly SqliteInMemoryDbFixture _db;

    public TestAuctionSnapshotIngestionRunOrchestrator(string authJson, string auctionsJson, SqliteInMemoryDbFixture db)
    {
        _authJson = authJson;
        _auctionsJson = auctionsJson;
        _db = db;
    }

    public AuctionSnapshotIngestionRunOrchestrator Create()
    {
        IConfiguration authConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Blizzard:ClientId"] = "test-client-id",
                ["Blizzard:ClientSecret"] = "test-client-secret",
            })
            .Build();

        var stubHandlerWithAuthJson = new StubHttpMessageHandler(_authJson, System.Net.HttpStatusCode.OK);

        var httpClientWithAuthStub = new HttpClient(stubHandlerWithAuthJson);

        var battleNetAuthClient = new BattleNetAuthClient(authConfig, httpClientWithAuthStub);


        var stubHandlerWithAuctionsJson = new StubHttpMessageHandler(_auctionsJson, System.Net.HttpStatusCode.OK);

        var httpClientWithAuctionsStub = new HttpClient(stubHandlerWithAuctionsJson);

        var wowApiClient = new WowApiClient(httpClientWithAuctionsStub);


        ILogger<AuctionSnapshotIngestionRunOrchestrator> logger = NullLogger<AuctionSnapshotIngestionRunOrchestrator>.Instance;

        IngestorDbContext dbContext = _db.CreateDbContext();

        var ingestionRunOrchestrator = new AuctionSnapshotIngestionRunOrchestrator(logger, dbContext, battleNetAuthClient, wowApiClient);
        return ingestionRunOrchestrator;

    }


}