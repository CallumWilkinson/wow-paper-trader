using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.VisualBasic;

public static class CommodityAuctionSnapshotTestFactory
{
    public async static Task AddOlderSnapshotToDbAsync(ApplicationDbContext arrangeDbContext, DateTime olderTime)
    {
        var olderIngestionRun = new IngestionRun();
        arrangeDbContext.IngestionRuns.Add(olderIngestionRun);
        await arrangeDbContext.SaveChangesAsync();

        var olderSnapshot = CreateOlderAuctionSnapshot(olderIngestionRun.Id, olderTime);
        arrangeDbContext.CommodityAuctionSnapshots.Add(olderSnapshot);
        await arrangeDbContext.SaveChangesAsync();
    }

    public async static Task AddNewerSnapshotToDbAsync(ApplicationDbContext arrangeDbContext, DateTime newerTime)
    {
        var newerIngestionRun = new IngestionRun();
        arrangeDbContext.IngestionRuns.Add(newerIngestionRun);
        await arrangeDbContext.SaveChangesAsync();

        var newerSnapshot = CreateNewerAuctionSnapshot(newerIngestionRun.Id, newerTime);
        arrangeDbContext.CommodityAuctionSnapshots.Add(newerSnapshot);
        await arrangeDbContext.SaveChangesAsync();
    }


    private static CommodityAuctionSnapshot CreateOlderAuctionSnapshot(
        long ingestionRunId,
        DateTime fetchedAtUtc,
        string apiEndPoint = "/commodities"
    )
    {
        var snapshot = new CommodityAuctionSnapshot(
            ingestionRunId,
            fetchedAtUtc,
            apiEndPoint
        );

        snapshot.AddAuction(
            new CommodityAuction(itemId: 2770, quantity: 10, unitPrice: 50, timeLeft: "SHORT")
        );

        snapshot.AddAuction(
            new CommodityAuction(itemId: 1234, quantity: 20, unitPrice: 999, timeLeft: "LONG")
        );

        return snapshot;
    }

    private static CommodityAuctionSnapshot CreateNewerAuctionSnapshot(
    long ingestionRunId,
    DateTime fetchedAtUtc,
    string apiEndPoint = "/commodities"
)
    {
        var snapshot = new CommodityAuctionSnapshot(
            ingestionRunId,
            fetchedAtUtc,
            apiEndPoint
        );

        snapshot.AddAuction(
            new CommodityAuction(itemId: 2770, quantity: 10, unitPrice: 120, timeLeft: "SHORT")
        );

        snapshot.AddAuction(
            new CommodityAuction(itemId: 2770, quantity: 5, unitPrice: 80, timeLeft: "MEDIUM")
        );

        snapshot.AddAuction(
            new CommodityAuction(itemId: 9999, quantity: 1, unitPrice: 1, timeLeft: "LONG")
        );

        return snapshot;
    }

}


