public sealed class CommodityAuctionSnapshotMapper
{
    public CommodityAuctionSnapshot MapToEntityFromDto(
        CommodityAuctionsResponseDto dto,
        long ingestionRunId,
        DateTime fetchedAtUtc,
        string endpoint
    )
    {
        CommodityAuctionSnapshot snapshot = new CommodityAuctionSnapshot(ingestionRunId, fetchedAtUtc, endpoint);

        foreach (CommodityAuctionDto commodityAuctionDto in dto.CommodityAuctions)
        {
            CommodityAuction commodityAuctionEntity = MapCommodityAuction(commodityAuctionDto);
            snapshot.AddAuction(commodityAuctionEntity);
        }

        return snapshot;
    }

    private static CommodityAuction MapCommodityAuction(CommodityAuctionDto commodityAuctionDto)
    {
        return new CommodityAuction(
            commodityAuctionDto.Item.Id,
            commodityAuctionDto.Quanity,
            commodityAuctionDto.UnitPrice,
            commodityAuctionDto.TimeLeft
        );
    }
}