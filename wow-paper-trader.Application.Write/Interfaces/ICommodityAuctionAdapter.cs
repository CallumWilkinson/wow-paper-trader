public interface ICommodityAuctionAdapter
{
    Task<CommodityAuctionSnapshotResult> GetCommodityAuctionsAsync(CancellationToken cancellationToken);
}


public sealed record CommodityAuctionSnapshotResult(
    DateTime DataReturnedAtUtc,
    string Endpoint,
    IReadOnlyList<CommodityAuctionRow> Auctions
);

public sealed record CommodityAuctionRow(
    long AuctionId,
    int ItemId,
    long Quantity,
    long UnitPrice,
    string TimeLeft);