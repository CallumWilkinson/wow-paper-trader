using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;
using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot.WowApiResult;
using WowPaperTrader.Domain.Features.Write.UpdateItems;

namespace WowPaperTrader.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<IngestionRunEntity> IngestionRuns { get; set; } = null!;

    public DbSet<CommodityAuctionSnapshotEntity> CommodityAuctionSnapshots { get; set; } = null!;

    public DbSet<CommodityAuctionEntity> CommodityAuctions { get; set; } = null!;

    public DbSet<ItemMetaDataEntity> ItemMetaData { get; set; } = null!;
}