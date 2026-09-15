using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.WowApiResult;
using WowPaperTrader.Application.Features.Write.UpdateItems;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

namespace WowPaperTrader.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
    : DbContext(dbContextOptions)
{
    public DbSet<IngestionRun> IngestionRuns { get; set; } = null!;

    public DbSet<CommodityAuctionSnapshot> CommodityAuctionSnapshots { get; set; } = null!;

    public DbSet<CommodityAuction> CommodityAuctions { get; set; } = null!;

    public DbSet<ItemMetaData> ItemMetaData { get; set; } = null!;

    public DbSet<AuctionMarket> AuctionMarkets { get; set; } = null!;

    public DbSet<AuctionMarketSnapshot> AuctionMarketSnapshots { get; set; } = null!;

    public DbSet<ItemMarketSnapshot> ItemMarketSnapshots { get; set; } = null!;

    public DbSet<CurrentPriceLevel> CurrentPriceLevels { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        //legacy schema
        modelBuilder.Entity<CommodityAuction>(entity =>
        {
            entity
                .HasIndex(auction => new
                {
                    auction.ItemId,
                    auction.CommodityAuctionSnapshotId
                })
                .HasDatabaseName("IX_CommodityAuctions_ItemId_CommodityAuctionSnapshotId")
                .IncludeProperties(auction => new
                {
                    auction.UnitPrice,
                    auction.Quantity
                });
        });
        
        //legacy schema
        modelBuilder.Entity<CommodityAuctionSnapshot>(entity =>
        {
            entity
                .HasIndex(snapshot => snapshot.FetchedAtUtc)
                .HasDatabaseName("IX_CommodityAuctionSnapshots_FetchedAtUtc");
        });
        
    }
}