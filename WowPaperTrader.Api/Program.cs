using System.Net;
using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Application.Features.Read.GetMetadata;
using WowPaperTrader.Application.Features.Read.ItemSearch;
using WowPaperTrader.Application.Features.Read.LowestPrice;
using WowPaperTrader.Application.Features.Read.MonthlyPriceQuantity;
using WowPaperTrader.Persistence;
using WowPaperTrader.Persistence.ReadServices;
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
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null);
        
        npgsqlOptions.CommandTimeout(300);
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