namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

public class ItemMarketSnapshot( long auctionMarketId, long itemId, long variantKey, DateTime observedAtUtc)

{
    public long AuctionMarketId { get; private set; } = auctionMarketId;
    
    public long ItemId { get; private set; } = itemId;
    
    public long VariantKey { get; private set; } = variantKey;
    
    public DateTime ObservedAtUtc { get; private set; } = observedAtUtc;
    
    //Navigation property
    public AuctionMarketSnapshot AuctionMarketSnapshot { get; private set; } = null;
    
    
}