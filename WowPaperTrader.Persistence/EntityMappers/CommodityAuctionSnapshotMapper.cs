using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;

namespace WowPaperTrader.Persistence.EntityMappers;

public static class CommodityAuctionSnapshotMapper
{
    public static CommodityAuctionSnapshot MapToEntity(
        WowApiResponse<AuctionSnapshot> apiResponse,
        long ingestionRunId
    )
    {
        var payload = apiResponse.Payload;
        var snapshot = new CommodityAuctionSnapshot(ingestionRunId, apiResponse.DataReturnedAtUtc, apiResponse.Endpoint);

        foreach (var auction in payload.Auctions)
        {
            var commodityAuctionEntity = MapCommodityAuction(auction);
            snapshot.AddAuction(commodityAuctionEntity);
        }

        return snapshot;
    }

    private static CommodityAuction MapCommodityAuction(AuctionSnapshotRow auctionSnapshotRow)
    {
        return new CommodityAuction(
            auctionSnapshotRow.ItemId,
            auctionSnapshotRow.Quantity,
            auctionSnapshotRow.UnitPrice,
            auctionSnapshotRow.TimeLeft
        );
    }
}