using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using wow_paper_trader.Ingestor;

public sealed class TestBuilderForIngestionRunOrchestrator
{
    private readonly string _authJson;
    private readonly string _auctionsJson;


    public TestBuilderForIngestionRunOrchestrator(string authJson, string auctionsJson)
    {
        _authJson = authJson;
        _auctionsJson = auctionsJson;
    }

    public IngestionRunOrchestrator CreateOrchestrator(IngestorDbContext dbContext)
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

        var httpClientWithAuctionsStub = new HttpClient(stubHandlerWithAuctionsJson)
        {
            BaseAddress = new Uri("https://example.test/")
        };

        var wowApiClient = new WowApiClient(httpClientWithAuctionsStub);


        ILogger<IngestionRunOrchestrator> logger = NullLogger<IngestionRunOrchestrator>.Instance;


        var ingestionRunOrchestrator = new IngestionRunOrchestrator(logger, dbContext, battleNetAuthClient, wowApiClient);
        return ingestionRunOrchestrator;

    }


}