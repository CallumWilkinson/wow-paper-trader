using System.Net;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Application.Read.Interfaces;
using WowPaperTrader.Application.Read.UseCases;
using WowPaperTrader.Infrastructure.Adapters;
using WowPaperTrader.Infrastructure.HttpClients;
using WowPaperTrader.Persistence;
using WowPaperTrader.Persistence.Queries;
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

builder.Services.AddScoped<GetCurrentLowestUnitPriceByItemIdUseCase>();
builder.Services.AddScoped<ICurrentLowestUnitPriceQuery, CurrentLowestUnitPriceQuery>();

builder.Services.AddScoped<GetMetadataAndPriceByItemIdUseCase>();
builder.Services.AddScoped<IItemMetadataAndPriceQuery, ItemMetadataAndPriceQuery>();

builder.Services.AddScoped<ItemSearchUseCase>();
builder.Services.AddScoped<IItemSearchQuery, ItemSearchQuery>();

builder.Services.AddScoped<UpdateItemMetaDataUseCase>();
builder.Services.AddScoped<IItemIdsWithoutMetadataQuery, ItemIdsWithoutMetadataQuery>();
builder.Services.AddScoped<IItemMetaDataApiAdapter, ItemMetaDataApiAdapter>();
builder.Services.AddScoped<IItemMetaDataRepository, ItemMetaDataRepository>();

builder.Services.AddHttpClient<BattleNetAuthClient>()
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        PooledConnectionLifetime = TimeSpan.FromMinutes(10),
    });

var wowApiBaseUrl = builder.Configuration["WowApi:BaseUrl"]
                    ?? throw new InvalidOperationException("WowApi:BaseUrl is missing.");

builder.Services.AddHttpClient<ItemMetaDataClient>(client =>
    {
        client.BaseAddress = new Uri(wowApiBaseUrl);
    })
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        PooledConnectionLifetime = TimeSpan.FromMinutes(10),
    });

builder.Services.AddHttpClient<ItemMediaClient>(client =>
    {
        client.BaseAddress = new Uri(wowApiBaseUrl);
    })
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        PooledConnectionLifetime = TimeSpan.FromMinutes(10),
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

