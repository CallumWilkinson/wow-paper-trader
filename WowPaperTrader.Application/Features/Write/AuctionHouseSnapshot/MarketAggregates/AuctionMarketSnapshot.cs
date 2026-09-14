namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

public class AuctionMarketSnapshot(long auctionMarketId, DateTime fetchedAtUtc, DateTime observedAtUtc, long ingestionRunId, string apiEndPoint)

{
    public long AuctionMarketId { get; private set; } = auctionMarketId;
    
    public DateTime ObservedAtUtc { get; private set; } = observedAtUtc;   
    
    public DateTime FetchedAtUtc { get; private set; } = fetchedAtUtc;
    
    public long IngestionRunId { get; private set; } = ingestionRunId;
    
    public string ApiEndPoint { get; private set; } = apiEndPoint;
    
    //Navigation property
    public AuctionMarket AuctionMarket { get; private set; } = null;

    //Navigation property
    public IngestionRun IngestionRun { get; private set; } = null;
    
    public List<ItemMarketSnapshot> ItemMarketSnapshots { get; } = new();

    public List<CurrentPriceLevel> CurrentPriceLevels { get; } = new();
}