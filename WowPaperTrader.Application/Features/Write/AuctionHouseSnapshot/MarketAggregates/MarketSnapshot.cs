namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

public class MarketSnapshot(long marketId, DateTime fetchedAtUtc, DateTime observedAtUtc, long ingestionRunId, string apiEndPoint)

{
    public long MarketId { get; private set; } = marketId;
    
    public DateTime ObservedAtUtc { get; private set; } = observedAtUtc;   
    
    public DateTime FetchedAtUtc { get; private set; } = fetchedAtUtc;
    
    public long IngestionRunId { get; private set; } = ingestionRunId;
    
    public string ApiEndPoint { get; private set; } = apiEndPoint;
    
    //Navigation property
    public Market Market { get; private set; } = null;

    //Navigation property
    public IngestionRun IngestionRun { get; private set; } = null;
    
    public List<ItemMarketSnapshot> ItemMarketSnapshots { get; } = new();

    public List<CurrentPriceLevel> CurrentPriceLevels { get; } = new();
    
}