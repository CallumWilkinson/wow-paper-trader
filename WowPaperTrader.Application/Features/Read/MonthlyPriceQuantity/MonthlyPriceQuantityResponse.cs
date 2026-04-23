namespace WowPaperTrader.Application.Features.Read.MonthlyPriceQuantity;

public class MonthlyPriceQuantityResponse
{
    public long ItemId { get; init; }

    public IReadOnlyList<PriceQuantityResponse> Rows { get; init; } = [];
    
}

public sealed class PriceQuantityResponse
{
    public long CommodityAuctionSnapshotId { get; init; }
    
    public DateTime FetchedAtUtc { get; init; }
    
    public long LowestUnitPrice { get; init; }
    
    public long TotalQuantityPosted  { get; init; }
}