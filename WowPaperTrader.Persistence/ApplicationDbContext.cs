using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.WowApiResult;
using WowPaperTrader.Application.Features.Write.UpdateItems;

namespace WowPaperTrader.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<IngestionRun> IngestionRuns { get; set; } = null!;

    public DbSet<CommodityAuctionSnapshot> CommodityAuctionSnapshots { get; set; } = null!;

    public DbSet<CommodityAuction> CommodityAuctions { get; set; } = null!;

    public DbSet<ItemMetaData> ItemMetaData { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
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

        modelBuilder.Entity<CommodityAuctionSnapshot>(entity =>
        {
            entity
                .HasIndex(snapshot => snapshot.FetchedAtUtc)
                .HasDatabaseName("IX_CommodityAuctionSnapshots_FetchedAtUtc");
        });
    }
}