namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

public class ItemMarketSnapshot( long auctionMarketId, long itemId, long variantKey, DateTime observedAtUtc)

{
    public long AuctionMarketId { get; private set; } = auctionMarketId;
    
    public long ItemId { get; private set; } = itemId;
    
    public long VariantKey { get; private set; } = variantKey;
    
    public DateTime ObservedAtUtc { get; private set; } = observedAtUtc;
    
    //Unfiltered aggregates
    public long ListingCount { get; private set; }
    
    public long TotalQuantity { get; private set; }
    
    public long MinimumUnitPrice { get; private set; }
    
    public decimal MeanUnitPrice { get; private set; }
    
    public decimal QuantityWeightedMeanUnitPrice { get; private set; }
    
    public long P10UnitPrice { get; private set; }
    
    public long P25UnitPrice { get; private set; }
    
    public long MedianUnitPrice { get; private set; }
    
    public long P75UnitPrice { get; private set; }
    
    public long P90UnitPrice { get; private set; }
    
    //Filtering
    public decimal LowerFenceUnitPrice { get; private set; }
    
    public decimal UpperFenceUnitPrice { get; private set; }
    
    public bool FilterApplied { get; private set; }
    
    //Filtered aggregates
    public long FilteredListingCount { get; private set; }
    
    public long FilteredTotalQuantity { get; private set; }
    
    public long? FilteredMinimumUnitPrice { get; private set; }
    
    public decimal? FilteredMeanUnitPrice { get; private set; }
    
    public decimal? FilteredQuantityWeightedMeanUnitPrice { get; private set; }
    
    //Navigation property
    public AuctionMarketSnapshot AuctionMarketSnapshot { get; private set; } = null;
    
    
}