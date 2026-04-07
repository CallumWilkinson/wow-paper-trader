using WowPaperTrader.Domain.Contracts;
using WowPaperTrader.Infrastructure.DTOs;

namespace WowPaperTrader.Infrastructure.ContractMappers;

public static class WowApiResultMapper
{
    public static WowApiResult<AuctionSnapshot> MapToContract(
        WowApiResult<CommodityAuctionsResponseDto> resultWithDto)
    {
        var dto = resultWithDto.Payload;

        var auctions = dto.CommodityAuctions
            .Select(MapSnapshotRow)
            .ToList();

        var snapshot = new AuctionSnapshot(auctions);

        return new WowApiResult<AuctionSnapshot>(
            snapshot,
            resultWithDto.DataReturnedAtUtc,
            resultWithDto.Endpoint
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