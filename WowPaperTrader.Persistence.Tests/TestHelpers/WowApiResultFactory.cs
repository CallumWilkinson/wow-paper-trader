using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;
using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot.WowApiResult;

namespace WowPaperTrader.Persistence.Tests.TestHelpers;

public static class WowApiResultFactory
{
    public static WowApiResult<AuctionSnapshot> Create()
    {
        var auctions = new List<AuctionSnapshotRow>
        {
            new(19012, 5, 250000, "LONG"),
            new(19020, 10, 120000, "SHORT")
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
            new(2770, 10, 50, "LONG"),
            new(1234, 20, 999, "SHORT")
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
            new(2770, 10, 120, "LONG"),
            new(2770, 5, 80, "SHORT"),
            new(9999, 1, 1, "MEDIUM")
        };

        var snapshot = new AuctionSnapshot(auctions);

        return new WowApiResult<AuctionSnapshot>(
            snapshot,
            newerTime,
            "/commodities"
        );
    }
}