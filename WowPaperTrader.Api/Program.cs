using System.Net;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Domain.Features.Read.GetMetadata;
using WowPaperTrader.Domain.Features.Read.ItemSearch;
using WowPaperTrader.Domain.Features.Read.LowestPrice;
using WowPaperTrader.Domain.Features.Write.UpdateItems;
using WowPaperTrader.Infrastructure.Adapters;
using WowPaperTrader.Infrastructure.HttpClients;
using WowPaperTrader.Persistence;
using WowPaperTrader.Persistence.ReadServices;
using WowPaperTrader.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("WowPaperTrader");
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<LowestPriceQueryHandler>();
builder.Services.AddScoped<ILowestPriceReadService, LowestPriceReadService>();

builder.Services.AddScoped<GetMetadataQueryHandler>();
builder.Services.AddScoped<MetadataReadService, MetadataReadService>();

builder.Services.AddScoped<ItemSearchQueryHandler>();
builder.Services.AddScoped<IItemSearchReadService, ItemSearchReadService>();

builder.Services.AddScoped<UpdateItemsCommandHandler>();
builder.Services.AddScoped<IItemIdsWithoutMetadataReadService, ItemIdsWithoutMetadataReadService>();
builder.Services.AddScoped<IItemMetadataApiAdapter, ItemMetadataApiAdapter>();
builder.Services.AddScoped<IItemMetadataRepository, ItemMetadataRepository>();

builder.Services.AddHttpClient<BattleNetAuthClient>()
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        PooledConnectionLifetime = TimeSpan.FromMinutes(10)
    });

var wowApiBaseUrl = builder.Configuration["WowApi:BaseUrl"]
                    ?? throw new InvalidOperationException("WowApi:BaseUrl is missing.");

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();