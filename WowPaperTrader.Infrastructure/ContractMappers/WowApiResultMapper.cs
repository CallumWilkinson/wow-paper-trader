using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;
using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot.ApiResponse;
using WowPaperTrader.Infrastructure.DTOs;

namespace WowPaperTrader.Infrastructure.ContractMappers;

public static class WowApiResultMapper
{
    public static WowApiResponse<AuctionSnapshot> MapToContract(
        WowApiResponse<CommodityAuctionsResponseDto> responseWithDto)
    {
        var dto = responseWithDto.Payload;

        var auctions = dto.CommodityAuctions
            .Select(MapSnapshotRow)
            .ToList();

        var snapshot = new AuctionSnapshot(auctions);

        return new WowApiResponse<AuctionSnapshot>(
            snapshot,
            responseWithDto.DataReturnedAtUtc,
            responseWithDto.Endpoint
        );
    }

    private static AuctionSnapshotRow MapSnapshotRow(CommodityAuctionDto auction)
    {
        return new AuctionSnapshotRow(
            auction.Item.Id,
            auction.Quantity,
            auction.UnitPrice,
            auction.TimeLeft
        );
    }
}