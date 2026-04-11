using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot.WowApiResult;

namespace WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;

public interface ICommodityAuctionApiAdapter
{
    Task<WowApiResult<AuctionSnapshot>> GetCommodityAuctionsSnapshotAsync(CancellationToken cancellationToken);
}