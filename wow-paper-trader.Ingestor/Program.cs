using wow_paper_trader.Ingestor;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<AuctionSnapshotIngestor>();

var host = builder.Build();
host.Run();
