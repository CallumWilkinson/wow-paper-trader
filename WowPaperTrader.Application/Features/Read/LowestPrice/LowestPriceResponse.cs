namespace WowPaperTrader.Application.Features.Read.LowestPrice;

public sealed class LowestPriceResponse
{
    public long ItemId { get; init; }

    public long UnitPrice { get; init; }

    public DateTime PriceTakenAtUtc { get; init; }
}