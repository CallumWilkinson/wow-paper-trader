using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;
using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot.ApiResponse;

namespace WowPaperTrader.Persistence.Tests.TestHelpers;

public static class WowApiResultFactory
{
    public static WowApiResponse<AuctionSnapshot> Create()
    {
        var auctions = new List<AuctionSnapshotRow>
        {
            new(19012, 5, 250000, "LONG"),
            new(19020, 10, 120000, "SHORT")
        };

        var snapshot = new AuctionSnapshot(auctions);

        return new WowApiResponse<AuctionSnapshot>(
            snapshot,
            DateTime.UtcNow,
            "/commodities"
        );
    }

    public static WowApiResponse<AuctionSnapshot> CreateOlderApiResult(DateTime olderTime)
    {
        var auctions = new List<AuctionSnapshotRow>
        {
            new(2770, 10, 50, "LONG"),
            new(1234, 20, 999, "SHORT")
        };


        var snapshot = new AuctionSnapshot(auctions);

        return new WowApiResponse<AuctionSnapshot>(
            snapshot,
            olderTime,
            "/commodities"
        );
    }

    public static WowApiResponse<AuctionSnapshot> CreateNewerApiResult(DateTime newerTime)
    {
        var auctions = new List<AuctionSnapshotRow>
        {
            new(2770, 10, 120, "LONG"),
            new(2770, 5, 80, "SHORT"),
            new(9999, 1, 1, "MEDIUM")
        };

        var snapshot = new AuctionSnapshot(auctions);

        return new WowApiResponse<AuctionSnapshot>(
            snapshot,
            newerTime,
            "/commodities"
        );
    }
}