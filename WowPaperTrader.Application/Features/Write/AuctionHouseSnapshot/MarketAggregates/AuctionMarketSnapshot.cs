namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

public class AuctionMarketSnapshot(long auctionMarketId, DateTime fetchedAtUtc, long ingestionRunId, string apiEndPoint)

{
    public long Id { get; private set; }
    
    public long AuctionMarketId { get; private set; } = auctionMarketId;

    //Navigation property
    public AuctionMarket AuctionMarket { get; private set; } = null;
    
    public DateTime FetchedAtUtc { get; private set; } = fetchedAtUtc;
    
    public long IngestionRunId { get; private set; } = ingestionRunId;

    //Navigation property
    public IngestionRun IngestionRun { get; private set; } = null;
    
    public string ApiEndPoint { get; private set; } = apiEndPoint;

    public List<ItemMarketSnapshot> ItemMarketSnapshots { get; } = new();

    public List<CurrentPriceLevel> CurrentPriceLevels { get; } = new();
}