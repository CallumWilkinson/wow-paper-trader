namespace WowPaperTrader.Domain.Features.Read.LowestPrice;

public sealed class CurrentLowestUnitPriceResponse
{
    public long ItemId { get; init; }

    public long UnitPrice { get; init; }

    public DateTime PriceTakenAtUtc { get; init; }
}