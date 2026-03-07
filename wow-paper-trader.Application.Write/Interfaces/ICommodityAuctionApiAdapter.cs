public interface ICommodityAuctionApiAdapter
{
    Task<WowApiResult<AuctionSnapshot>> GetCommodityAuctionsSnapshotAsync(CancellationToken cancellationToken);
}


