public interface ICommodityAuctionApiAdapter
{
    Task<CommodityAuctionSnapshot> GetCommodityAuctionsSnapshotAsync(CancellationToken cancellationToken);
}
