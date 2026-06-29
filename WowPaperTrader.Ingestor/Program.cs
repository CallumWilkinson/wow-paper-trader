using System.Net;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot;
using WowPaperTrader.Application.Features.Write.Helpers;
using WowPaperTrader.Application.Features.Write.UpdateItems;
using WowPaperTrader.Infrastructure.Adapters;
using WowPaperTrader.Infrastructure.HttpClients;
using WowPaperTrader.Ingestor;
using WowPaperTrader.Persistence;
using WowPaperTrader.Persistence.Helpers;
using WowPaperTrader.Persistence.ReadServices;
using WowPaperTrader.Persistence.Repositories;

var builder = Host.CreateApplicationBuilder(args);

//add db connection strings
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("WowPaperTrader");
    
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("ConnectionStrings:WowPaperTrader is missing.");
    }
    
    options.UseNpgsql(connectionString);
});

//add services
builder.Services.AddSingleton<AuctionDataIngestionJob>();
builder.Services.AddSingleton<UpdateItemMetadataJob>();

builder.Services.AddScoped<PostAuctionDataCommandHandler>();
builder.Services.AddScoped<ICommodityAuctionApiAdapter, CommodityAuctionApiAdapter>();
builder.Services.AddScoped<ICommodityAuctionRepository, CommodityAuctionRepository>();

builder.Services.AddScoped<UpdateItemsCommandHandler>();
builder.Services.AddScoped<IItemIdsWithoutMetadataReadService, ItemIdsWithoutMetadataReadService>();
builder.Services.AddScoped<IItemMetadataApiAdapter, ItemMetadataApiAdapter>();
builder.Services.AddScoped<IItemMetadataRepository, ItemMetadataRepository>();

//can delete? cos im no longer using azure??
builder.Services.AddScoped<IDatabaseSizeGuard, DatabaseSizeGuard>();

//add http clients to call external apis
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

builder.Services.AddHttpClient<ItemMetaDataClient>(client => { client.BaseAddress = new Uri(wowApiBaseUrl); })
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        PooledConnectionLifetime = TimeSpan.FromMinutes(10)
    });

builder.Services.AddHttpClient<ItemMediaClient>(client => { client.BaseAddress = new Uri(wowApiBaseUrl); })
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        PooledConnectionLifetime = TimeSpan.FromMinutes(10)
    });

//build
using var host = builder.Build();

await host.StartAsync();

try
{
    var applicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
    
    var stoppingToken = applicationLifetime.ApplicationStopping;
    
    var mode = args.FirstOrDefault()?.ToLowerInvariant();

    var exitCode = mode switch
    {
        "auctions" => await host.Services.GetRequiredService<AuctionDataIngestionJob>().RunAsync(stoppingToken),

        "metadata" => await RunAuctionsAndMetadata.RunAsync(host.Services, stoppingToken),
    
        _ => throw new ArgumentException(        $"Invalid mode: {mode}. Choose one of: " +
                                                 "auctions (updates the database with a new auction snapshot), " +
                                                 "or metadata (updates auctions and then any new item metadata).")
    };

    Environment.ExitCode = exitCode;
}
finally
{
    await host.StopAsync();
}




