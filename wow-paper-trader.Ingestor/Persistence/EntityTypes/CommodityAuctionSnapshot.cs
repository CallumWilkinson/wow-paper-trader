//note to self - when I create and save an entity instance of type <CommodityAuctionSnapshot>, 1 database row is created in the CommodityAuctionSnapshots table (the list in the db set)

public sealed class CommodityAuctionSnapshot
{
    public int Id { get; private set; }

    public int IngestionRunId { get; private set; }

    //this is a navigation property, it is not created as a column in the db
    //it is for linking this entity type to the ingestionRun in code
    public IngestionRun IngestionRun { get; private set; } = null!;

    public DateTime FetchedAtUtc { get; private set; }

    public string ApiEndPoint { get; private set; } = string.Empty;

    //navigation collection property becuase this is a list of custom types
    public List<CommodityAuction> CommodityAuctions { get; private set; } = new List<CommodityAuction>();

    public CommodityAuctionSnapshot(int ingestionRunId, DateTime fetchedAtUtc, string apiEndPoint)
    {
        ingestionRunId = IngestionRunId;
        fetchedAtUtc = FetchedAtUtc;
        apiEndPoint = ApiEndPoint;
    }


}