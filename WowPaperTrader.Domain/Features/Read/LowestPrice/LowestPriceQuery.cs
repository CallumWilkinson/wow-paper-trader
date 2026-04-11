using WowPaperTrader.Domain.Architecture;

namespace WowPaperTrader.Domain.Features.Read.LowestPrice;

public sealed class LowestPriceQuery(long itemId) : IQuery<LowestPriceResponse>
{
    public readonly long ItemId = itemId;
}