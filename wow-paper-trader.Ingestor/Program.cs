using Microsoft.EntityFrameworkCore;
using wow_paper_trader.Ingestor;
using System.Net;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("WowPaperTrader");
    options.UseSqlServer(connectionString);
});

builder.Services.AddHostedService<IngestionRunBackgroundService>();

builder.Services.AddScoped<IngestionRunUseCase>();
builder.Services.AddScoped<ICommodityAuctionApiAdapter, CommodityAuctionApiAdapter>();
builder.Services.AddScoped<ICommodityAuctionRepository, CommodityAuctionRepository>();

builder.Services.AddHttpClient<BattleNetAuthClient>()
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        PooledConnectionLifetime = TimeSpan.FromMinutes(10),
    });

var wowApiBaseUrl = builder.Configuration["WowApi:BaseUrl"]
?? throw new InvalidOperationException("WowApi:BaseUrl is missing.");

builder.Services.AddHttpClient<CommodityAuctionClient>(client =>
{
    client.BaseAddress = new Uri(wowApiBaseUrl);
})
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        PooledConnectionLifetime = TimeSpan.FromMinutes(10),
    });

builder.Services.Configure<HostOptions>(options =>
{
    options.ShutdownTimeout = TimeSpan.FromMinutes(10);
});

var host = builder.Build();
host.Run();
