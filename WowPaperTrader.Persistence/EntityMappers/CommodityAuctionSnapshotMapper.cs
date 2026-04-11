using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot;
using WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot.WowApiResult;

namespace WowPaperTrader.Persistence.EntityMappers;

public static class CommodityAuctionSnapshotMapper
{
    public static CommodityAuctionSnapshotEntity MapToEntity(
        WowApiResult<AuctionSnapshot> apiResult,
        long ingestionRunId
    )
    {
        var payload = apiResult.Payload;
        var snapshot = new CommodityAuctionSnapshotEntity(ingestionRunId, apiResult.DataReturnedAtUtc, apiResult.Endpoint);

        foreach (var auction in payload.Auctions)
        {
            var commodityAuctionEntity = MapCommodityAuction(auction);
            snapshot.AddAuction(commodityAuctionEntity);
        }

        return snapshot;
    }

    private static CommodityAuctionEntity MapCommodityAuction(AuctionSnapshotRow auctionSnapshotRow)
    {
        return new CommodityAuctionEntity(
            auctionSnapshotRow.ItemId,
            auctionSnapshotRow.Quantity,
            auctionSnapshotRow.UnitPrice,
            auctionSnapshotRow.TimeLeft
        );
    }
}