namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates.AggregateResult;

public sealed record CommodityListing(
    long ItemId,
    long Quantity,
    long UnitPrice
    )
{
    

}