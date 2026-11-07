using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates.AggregateResult;

namespace WowPaperTrader.Application.Tests.UnitTests.Write.TestData;

public static class MarketAggregatorTestData
{
    public static CommodityListing CreateCommodityListing(long itemId = 1001,
        long quantity = 1,
        long unitPrice = 50)
    {
        return new CommodityListing(
            itemId,
            quantity,
            unitPrice
        );
    }
    
}