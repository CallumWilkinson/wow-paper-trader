using System.Net;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Application.Features.Read.GetMetadata;
using WowPaperTrader.Application.Features.Read.ItemSearch;
using WowPaperTrader.Application.Features.Read.LowestPrice;
using WowPaperTrader.Application.Features.Read.MonthlyPriceQuantity;
using WowPaperTrader.Application.Features.Write.UpdateItems;
using WowPaperTrader.Infrastructure.Adapters;
using WowPaperTrader.Infrastructure.HttpClients;
using WowPaperTrader.Persistence;
using WowPaperTrader.Persistence.ReadServices;
using WowPaperTrader.Persistence.Repositories;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Database connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("WowPaperTrader");
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
        
        sqlServerOptions.CommandTimeout(300);
    });
});

//Rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(_ =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: "global-api-limit",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                    PermitLimit = 60,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    AutoReplenishment = true
            });
    });
});

//Services and handlers for controller end points
builder.Services.AddScoped<LowestPriceQueryHandler>();
builder.Services.AddScoped<ILowestPriceReadService, LowestPriceReadService>();

builder.Services.AddScoped<GetMetadataQueryHandler>();
builder.Services.AddScoped<IMetadataReadService, MetadataReadService>();

builder.Services.AddScoped<ItemSearchQueryHandler>();
builder.Services.AddScoped<IItemSearchReadService, ItemSearchReadService>();

builder.Services.AddScoped<MonthlyPriceQuantityQueryHandler>();
builder.Services.AddScoped<IMonthlyPriceQuantityReadService, MonthlyPriceQuantityReadService>();

builder.Services.AddScoped<UpdateItemsCommandHandler>();
builder.Services.AddScoped<IItemIdsWithoutMetadataReadService, ItemIdsWithoutMetadataReadService>();
builder.Services.AddScoped<IItemMetadataApiAdapter, ItemMetadataApiAdapter>();
builder.Services.AddScoped<IItemMetadataRepository, ItemMetadataRepository>();

//Http clients for external api calls
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

//CORS
var configuredOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

var allowedOrigins = configuredOrigins
    .Where(origin => !string.IsNullOrWhiteSpace(origin))
    .Select(origins => origins.Trim().TrimEnd('/'))
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToArray();

if (allowedOrigins.Length == 0)
{
    throw new InvalidOperationException("No CORS allowed origins configured.");
}


builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendCorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

//Middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("FrontendCorsPolicy");

app.UseRateLimiter();

app.MapControllers();

app.MapGet("/health", () => Results.Ok("OK"));

app.Run();