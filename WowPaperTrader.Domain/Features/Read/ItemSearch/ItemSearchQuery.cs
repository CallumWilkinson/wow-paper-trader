using WowPaperTrader.Domain.Architecture;

namespace WowPaperTrader.Domain.Features.ItemSearch;

public sealed class ItemSearchQuery(string itemName) : IQuery<List<ItemSearchResponse>>
{
    public readonly string ItemName = itemName;
}