namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

public class CurrentPriceLevel(long auctionMarketSnapshotId, long itemId, long variantKey, long unitPrice, long totalQuantity, long listingCount)
{
    public long Id { get; private set; }
    
    public long AuctionMarketSnapshotId { get; private set; } = auctionMarketSnapshotId;
    
    //Navigation property
    public AuctionMarketSnapshot AuctionMarketSnapshot { get; private set; } = null;
    
    public long ItemId { get; private set; } = itemId;
    
    public long VariantKey { get; private set; } = variantKey;
    
    public long UnitPrice { get; private set; } = unitPrice;
    
    public long TotalQuantity { get; private set; } = totalQuantity;
    
    public long ListingCount { get; private set; } = listingCount;
}