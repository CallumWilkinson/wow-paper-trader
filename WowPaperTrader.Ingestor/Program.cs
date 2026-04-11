using System.Net;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot;
using WowPaperTrader.Infrastructure.Adapters;
using WowPaperTrader.Infrastructure.HttpClients;
using WowPaperTrader.Ingestor;
using WowPaperTrader.Persistence;
using WowPaperTrader.Persistence.Repositories;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("WowPaperTrader");
    options.UseSqlServer(connectionString);
});

builder.Services.AddHostedService<AuctionDataBackgroundService>();

builder.Services.AddScoped<PostAuctionDataCommandHandler>();
builder.Services.AddScoped<ICommodityAuctionApiAdapter, CommodityAuctionApiAdapter>();
builder.Services.AddScoped<ICommodityAuctionRepository, CommodityAuctionRepository>();

builder.Services.AddHttpClient<BattleNetAuthClient>()
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        PooledConnectionLifetime = TimeSpan.FromMinutes(10)
    });

var wowApiBaseUrl = builder.Configuration["WowApi:BaseUrl"]
                    ?? throw new InvalidOperationException("WowApi:BaseUrl is missing.");

builder.Services.AddHttpClient<CommodityAuctionClient>(client => { client.BaseAddress = new Uri(wowApiBaseUrl); })
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        PooledConnectionLifetime = TimeSpan.FromMinutes(10)
    });

builder.Services.Configure<HostOptions>(options => { options.ShutdownTimeout = TimeSpan.FromMinutes(10); });

var host = builder.Build();
host.Run();