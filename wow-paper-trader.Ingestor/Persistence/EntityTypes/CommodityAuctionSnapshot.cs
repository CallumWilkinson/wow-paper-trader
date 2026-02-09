//note to self - when I create and save an entity instance of type <CommodityAuctionSnapshot>, 1 database row is created in the CommodityAuctionSnapshots table (the list in the db set)

public sealed class CommodityAuctionSnapshot
{
    public long Id { get; private set; }

    public long IngestionRunId { get; private set; }

    //this is a navigation property, it is not created as a column in the db
    //it is for linking this entity type to the ingestionRun in code
    //one to one relationship
    public IngestionRun IngestionRun { get; private set; } = null!;

    public DateTime FetchedAtUtc { get; private set; }

    public string ApiEndPoint { get; private set; } = string.Empty;

    //navigation collection property becuase this is a list of custom types
    //one to many relationship
    public List<CommodityAuction> CommodityAuctions { get; private set; } = new List<CommodityAuction>();

    public CommodityAuctionSnapshot(long ingestionRunId, DateTime fetchedAtUtc, string apiEndPoint)
    {
        ingestionRunId = IngestionRunId;
        fetchedAtUtc = FetchedAtUtc;
        apiEndPoint = ApiEndPoint;
    }


}