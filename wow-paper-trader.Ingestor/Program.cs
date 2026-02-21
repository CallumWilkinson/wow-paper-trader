using Microsoft.EntityFrameworkCore;
using wow_paper_trader.Ingestor;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<IngestorDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("WowPaperTrader");
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<AuctionSnapshotIngestionRunOrchestrator>();
builder.Services.AddHostedService<AuctionSnapshotIngestor>();

builder.Services.AddHttpClient<BattleNetAuthClient>()
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        PooledConnectionLifetime = TimeSpan.FromMinutes(10),
    });

builder.Services.AddHttpClient<WowApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["WowApi:BaseUrl"]!);
})
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        PooledConnectionLifetime = TimeSpan.FromMinutes(10),
    });


var host = builder.Build();
host.Run();
