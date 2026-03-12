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
}