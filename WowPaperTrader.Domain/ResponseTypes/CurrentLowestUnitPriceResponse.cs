namespace WowPaperTrader.Domain.ResponseTypes;

public sealed class CurrentLowestUnitPriceResponse
{
    public long ItemId { get; init; }

    public long UnitPrice { get; init; }

    public DateTime PriceTakenAtUtc { get; init; }
}