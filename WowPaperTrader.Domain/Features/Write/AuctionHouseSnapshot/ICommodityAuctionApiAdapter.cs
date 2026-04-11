using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot.ApiResponse;

namespace WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;

public interface ICommodityAuctionApiAdapter
{
    Task<WowApiResponse<AuctionSnapshot>> GetCommodityAuctionsSnapshotAsync(CancellationToken cancellationToken);
}