using WowPaperTrader.Application.Architecture;

namespace WowPaperTrader.Application.Features.Read.LowestPrice;

public sealed class LowestPriceQuery(long itemId) : IQuery<LowestPriceResponse>
{
    public readonly long ItemId = itemId;
}