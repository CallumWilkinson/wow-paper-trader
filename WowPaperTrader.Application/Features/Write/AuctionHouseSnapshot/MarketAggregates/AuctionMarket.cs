namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

public sealed class AuctionMarket(string region, AuctionMarketType auctionMarketType, long? connectedRealmId, string displayName)
{
    public long Id { get; private set; }
    
    public string Region { get; private set; } = region;
    
    public AuctionMarketType AuctionMarketType { get; private set; } = auctionMarketType;
    
    public long? ConnectedRealmId { get; private set; } = connectedRealmId;
    
    public string DisplayName { get; private set; } = displayName;

    public List<AuctionMarketSnapshot> MarketSnapshots { get; } = new();
}