using WowPaperTrader.Application.Read.Contracts;

namespace WowPaperTrader.Persistence.Tests.TestHelpers;

public static class WowApiResultFactory
{
    public static WowApiResult<AuctionSnapshot> Create()
    {
        var auctions = new List<AuctionSnapshotRow>
        {
            new AuctionSnapshotRow(19012, 5, 250000, "LONG"),
            new AuctionSnapshotRow(19020, 10, 120000, "SHORT")
        };

        var snapshot = new AuctionSnapshot(auctions);

        return new WowApiResult<AuctionSnapshot>(
            snapshot,
            DateTime.UtcNow,
            "/commodities"
        );
    }

    public static WowApiResult<AuctionSnapshot> CreateOlderApiResult(DateTime olderTime)
    {
        var auctions = new List<AuctionSnapshotRow>
        {
            new AuctionSnapshotRow(2770, 10, 50, "LONG"),
            new AuctionSnapshotRow(1234, 20, 999, "SHORT")
        };


        var snapshot = new AuctionSnapshot(auctions);

        return new WowApiResult<AuctionSnapshot>(
            snapshot,
            olderTime,
            "/commodities"
        );
    }

    public static WowApiResult<AuctionSnapshot> CreateNewerApiResult(DateTime newerTime)
    {
        var auctions = new List<AuctionSnapshotRow>
        {
            new AuctionSnapshotRow(2770, 10, 120, "LONG"),
            new AuctionSnapshotRow(2770, 5, 80, "SHORT"),
            new AuctionSnapshotRow(9999, 1, 1, "MEDIUM"),
        };

        var snapshot = new AuctionSnapshot(auctions);

        return new WowApiResult<AuctionSnapshot>(
            snapshot,
            newerTime,
            "/commodities"
        );
    }
}