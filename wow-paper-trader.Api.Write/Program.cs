using System.Net;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("WowPaperTrader");
    options.UseSqlServer(connectionString);
});

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
