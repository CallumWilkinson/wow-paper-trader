namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates.AggregateResult;

public sealed record CommodityAggregate(
    long ItemId,
    long VariantKey,
    long MinimumUnitPrice,
    long TotalQuantity,
    long ListingCount
    )
{
    
}