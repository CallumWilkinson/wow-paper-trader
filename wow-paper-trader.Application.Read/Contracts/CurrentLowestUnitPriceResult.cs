public sealed class CurrentLowestUnitPriceResult
{
    public long ItemId { get; init; }

    public long UnitPrice { get; init; }

    public DateTime PriceTakenAtUtc { get; init; }
}