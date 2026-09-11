namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

public class ItemMarketSnapshot(long auctionMarketSnapshotId, long itemId, long variantKey)

{
    public long Id { get; private set; }
    
    public long AuctionMarketSnapshotId { get; private set; } = auctionMarketSnapshotId;
    
    //Navigation property
    public AuctionMarketSnapshot AuctionMarketSnapshot { get; private set; } = null;
    
    public long ItemId { get; private set; } = itemId;
    
    public long VariantKey { get; private set; } = variantKey;
}