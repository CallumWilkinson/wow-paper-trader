using WowPaperTrader.Domain.Contracts;

namespace WowPaperTrader.Domain.Interfaces;

public interface ICommodityAuctionApiAdapter
{
    Task<WowApiResult<AuctionSnapshot>> GetCommodityAuctionsSnapshotAsync(CancellationToken cancellationToken);
}