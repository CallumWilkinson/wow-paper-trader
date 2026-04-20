using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.WowApiResult;

namespace WowPaperTrader.Persistence.EntityMappers;

public static class CommodityAuctionSnapshotMapper
{
    public static CommodityAuctionSnapshot MapToEntity(
        WowApiResult<AuctionSnapshot> apiResult,
        long ingestionRunId
    )
    {
        var payload = apiResult.Payload;
        
        var dataReturnedAtUtc = DateTime.SpecifyKind(apiResult.DataReturnedAtUtc, DateTimeKind.Utc);
        
        var snapshot = new CommodityAuctionSnapshot(ingestionRunId, dataReturnedAtUtc, apiResult.Endpoint);

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