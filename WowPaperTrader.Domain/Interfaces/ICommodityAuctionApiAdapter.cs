using WowPaperTrader.Application.Read.Contracts;

namespace WowPaperTrader.Application.Read.Interfaces;

public interface ICommodityAuctionApiAdapter
{
    Task<WowApiResult<AuctionSnapshot>> GetCommodityAuctionsSnapshotAsync(CancellationToken cancellationToken);
}