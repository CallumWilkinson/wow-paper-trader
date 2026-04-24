namespace WowPaperTrader.Application.Features.Read.MonthlyPriceQuantity;

public class MonthlyPriceQuantityResponse
{
    public long ItemId { get; init; }

    public List<PriceQuantityResponse> PriceQuantityResponses { get; init; } = new();
}

public sealed class PriceQuantityResponse
{
    public long CommodityAuctionSnapshotId { get; init; }
    
    public DateTime FetchedAtUtc { get; init; }
    
    public long LowestUnitPrice { get; init; }
    
    public long TotalQuantityPosted  { get; init; }
}