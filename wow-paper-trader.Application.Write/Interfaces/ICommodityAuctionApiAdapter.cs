public interface ICommodityAuctionApiAdapter
{
    Task<WowApiResult<AuctionSnapshot>> GetCommodityAuctionsSnapshotAsync(CancellationToken cancellationToken);
}

public sealed record AuctionSnapshot(
    IReadOnlyList<AuctionSnapshotRow> Auctions
);

public sealed record AuctionSnapshotRow(
    long ItemId,
    long Quantity,
    long UnitPrice,
    string TimeLeft
);
