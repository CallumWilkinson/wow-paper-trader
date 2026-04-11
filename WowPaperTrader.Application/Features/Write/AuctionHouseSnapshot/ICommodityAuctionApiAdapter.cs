using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.WowApiResult;

namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot;

public interface ICommodityAuctionApiAdapter
{
    Task<WowApiResult<AuctionSnapshot>> GetCommodityAuctionsSnapshotAsync(CancellationToken cancellationToken);
}