//note to self - an instance of this class is 1 row in the table called CommodityAuctionSnapshots

namespace WowPaperTrader.Domain.Features.Write.AuctionHouseSnapshot.ApiResponse;

public sealed class CommodityAuctionSnapshotEntity
{
    public CommodityAuctionSnapshotEntity(long ingestionRunId, DateTime fetchedAtUtc, string apiEndPoint)
    {
        IngestionRunId = ingestionRunId;
        FetchedAtUtc = fetchedAtUtc;
        ApiEndPoint = apiEndPoint;
    }

    public long Id { get; private set; }

    public long IngestionRunId { get; private set; }

    //this is a navigation property, it is not created as a column in the db
    //it is for linking this entity type to the ingestionRun in code
    //one to one relationship
    public IngestionRunEntity IngestionRunEntity { get; private set; } = null!;

    public DateTime FetchedAtUtc { get; private set; }

    public string ApiEndPoint { get; private set; } = string.Empty;

    //navigation collection property becuase this is a list of custom types
    //one to many relationship
    public List<CommodityAuctionEntity> CommodityAuctions { get; } = new();

    //used when we loop the DTO during mapping
    public void AddAuction(CommodityAuctionEntity commodityAuctionEntity)
    {
        CommodityAuctions.Add(commodityAuctionEntity);
    }
}