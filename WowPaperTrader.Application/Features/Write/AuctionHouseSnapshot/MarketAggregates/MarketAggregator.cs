using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates.AggregateResult;

namespace WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;

public static class MarketAggregator
{
    public static IReadOnlyList<CommodityAggregate> Aggregate(IEnumerable<CommodityListing> listings)
    {
        ArgumentNullException.ThrowIfNull(listings);

        var normalizedListings = listings.ToList();
        
        foreach (var listing in normalizedListings)
        {
            if (listing is null)
                throw new ArgumentException("A listing cannot be null");
            
            if (listing.ItemId <= 0)
                throw new ArgumentException("ItemId must be positive.");

            if (listing.UnitPrice <= 0)
                throw new ArgumentException("UnitPrice must be positive.");

            if (listing.Quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");
        }
        
        return normalizedListings
            .GroupBy(listing => listing.ItemId)
            .Select(group => new CommodityAggregate(
                ItemId: group.Key,
                VariantKey: 0,
                MinimumUnitPrice: group.Min(x => x.UnitPrice),
                TotalQuantity: group.Sum(x => x.Quantity),
                ListingCount: group.LongCount()
                )).ToList();
    }
}