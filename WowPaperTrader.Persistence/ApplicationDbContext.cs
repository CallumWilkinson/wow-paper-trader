using Microsoft.EntityFrameworkCore;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.WowApiResult;
using WowPaperTrader.Application.Features.Write.UpdateItems;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

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

    public DbSet<AuctionMarket> AuctionMarkets { get; set; } = null!;

    public DbSet<AuctionMarketSnapshot> AuctionMarketSnapshots { get; set; } = null!;

    public DbSet<ItemMarketSnapshot> ItemMarketSnapshots { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
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
        
        //new schema
        modelBuilder.Entity<AuctionMarket>(entity =>
        {
            entity.Property(auctionMarket => auctionMarket.AuctionMarketType).HasConversion<int>();

            entity.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_AuctionMarkets_AuctionMarketType_ConnectedRealmId",
                    """
                    (
                        ("AuctionMarketType" = 1 AND "ConnectedRealmId" IS NULL)
                        OR
                        ("AuctionMarketType" = 2 AND "ConnectedRealmId" IS NOT NULL)
                    )
                    """
                );
            });

            entity
                .HasIndex(auctionMarket => auctionMarket.Region)
                .IsUnique()
                .HasDatabaseName("UX_AuctionMarkets_Region_RegionalCommodities")
                .HasFilter("\"AuctionMarketType\" = 1");
            
            entity.HasIndex(auctionMarket => new { auctionMarket.ConnectedRealmId, auctionMarket.Region })
                .IsUnique()
                .HasDatabaseName(
                    "UX_AuctionMarkets_Region_ConnectedRealmId")
                .HasFilter("\"AuctionMarketType\" = 2");

            entity.HasData(new
            {
                Id = 1L,
                Region = "US",
                AuctionMarketType = AuctionMarketType.Commodity,
                ConnectedRealmId = (long?)null,
                DisplayName = "US Commodities"
            });
        });

        modelBuilder.Entity<AuctionMarketSnapshot>(entity =>
        {
            entity.HasKey(snapshot => new
            {
                snapshot.AuctionMarketId,
                snapshot.ObservedAtUtc
            });

            entity
                .HasOne(snapshot => snapshot.AuctionMarket)
                .WithMany(market => market.MarketSnapshots)
                .HasForeignKey(snapshot => snapshot.AuctionMarketId);

            entity
                .HasOne(snapshot => snapshot.IngestionRun)
                .WithMany(run => run.MarketSnapshots)
                .HasForeignKey(snapshot => snapshot.IngestionRunId);
        });

        modelBuilder.Entity<ItemMarketSnapshot>(entity =>
        {
            entity.HasKey(snapshot => new
            {
                snapshot.AuctionMarketId,
                snapshot.ItemId,
                snapshot.VariantKey,
                snapshot.ObservedAtUtc
            });

            entity
                .HasOne(snapshot => snapshot.AuctionMarketSnapshot)
                .WithMany(snapshot => snapshot.ItemMarketSnapshots)
                .HasForeignKey(snapshot => new
                {
                    snapshot.AuctionMarketId,
                    snapshot.ObservedAtUtc
                });

            entity
                .Property(snapshot => snapshot.LowerFenceUnitPrice)
                .HasPrecision(28, 6);
            
            entity
                .Property(snapshot => snapshot.UpperFenceUnitPrice)
                .HasPrecision(28, 6);

            entity
                .Property(snapshot => snapshot.FilteredMeanUnitPrice)
                .HasPrecision(28, 6);

            entity
                .Property(snapshot => snapshot.FilteredQuantityWeightedMeanUnitPrice)
                .HasPrecision(28, 6);
        });
    }
}