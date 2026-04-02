using Microsoft.EntityFrameworkCore;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

