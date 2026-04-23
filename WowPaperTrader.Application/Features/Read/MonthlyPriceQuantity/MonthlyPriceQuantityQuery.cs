using WowPaperTrader.Application.Architecture;

namespace WowPaperTrader.Application.Features.Read.MonthlyPriceQuantity;

public sealed class MonthlyPriceQuantityQuery(long itemId) : IQuery<MonthlyPriceQuantityResponse>
{
    public readonly long ItemId = itemId;
}