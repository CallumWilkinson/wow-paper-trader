using Microsoft.EntityFrameworkCore;
using wow_paper_trader.Ingestor;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<IngestorDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("WowPaperTrader");
    options.UseSqlServer(connectionString);
})

builder.Services.AddHostedService<AuctionSnapshotIngestor>();

var host = builder.Build();
host.Run();
