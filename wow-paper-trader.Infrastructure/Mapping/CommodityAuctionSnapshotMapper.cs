public sealed class CommodityAuctionSnapshotMapper
{
    public CommodityAuctionSnapshot MapToEntity(
        WowApiResult<CommodityAuctionSnapshot> apiResult,
        long ingestionRunId,
        DateTime fetchedAtUtc,
        string endpoint
    )
    {
        var snapshot = new CommodityAuctionSnapshot(ingestionRunId, fetchedAtUtc, endpoint);

        foreach (CommodityAuctionDto commodityAuctionDto in dto.CommodityAuctions)
        {
            var commodityAuctionEntity = MapCommodityAuction(commodityAuctionDto);
            snapshot.AddAuction(commodityAuctionEntity);
        }

        return snapshot;
    }

    private static CommodityAuction MapCommodityAuction(CommodityAuctionDto commodityAuctionDto)
    {
        return new CommodityAuction(
            commodityAuctionDto.Item.Id,
            commodityAuctionDto.Quantity,
            commodityAuctionDto.UnitPrice,
            commodityAuctionDto.TimeLeft
        );
    }
}
