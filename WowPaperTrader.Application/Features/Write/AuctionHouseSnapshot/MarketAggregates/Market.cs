namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

public sealed class Market(string region, MarketType marketType, long? connectedRealmId, string displayName)
{
    public long Id { get; private set; }
    
    public string Region { get; private set; } = region;
    
    public MarketType MarketType { get; private set; } = marketType;
    
    public long? ConnectedRealmId { get; private set; } = connectedRealmId;
    
    public string DisplayName { get; private set; } = displayName;

    public List<MarketSnapshot> MarketSnapshots { get; } = new();
}