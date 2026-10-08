namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

public class CurrentPriceLevel(long marketId, long itemId, long variantKey, long unitPrice, long totalQuantity, long listingCount, DateTime observedAtUtc)
{
    public long MarketId { get; private set; } = marketId;
    
    //Navigation property
    public MarketSnapshot MarketSnapshot { get; private set; } = null;
    
    public long ItemId { get; private set; } = itemId;
    
    public long VariantKey { get; private set; } = variantKey;
    
    public long UnitPrice { get; private set; } = unitPrice;

    public DateTime ObservedAtUtc { get; private set; } = observedAtUtc;
    
    public long TotalQuantity { get; private set; } = totalQuantity;
    
    public long ListingCount { get; private set; } = listingCount;
}