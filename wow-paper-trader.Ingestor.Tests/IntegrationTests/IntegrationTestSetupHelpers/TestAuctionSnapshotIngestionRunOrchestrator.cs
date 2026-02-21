using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

public sealed class TestAuctionSnapshotIngestionRunBuilder
{
    private readonly string _authJson;
    private readonly string _auctionsJson;


    public TestAuctionSnapshotIngestionRunBuilder(string authJson, string auctionsJson)
    {
        _authJson = authJson;
        _auctionsJson = auctionsJson;
    }

    public AuctionSnapshotIngestionRunOrchestrator CreateOrchestrator(IngestorDbContext dbContext)
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


        var ingestionRunOrchestrator = new AuctionSnapshotIngestionRunOrchestrator(logger, dbContext, battleNetAuthClient, wowApiClient);
        return ingestionRunOrchestrator;

    }


}