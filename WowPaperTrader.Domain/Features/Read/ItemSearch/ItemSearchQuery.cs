using WowPaperTrader.Domain.Architecture;

namespace WowPaperTrader.Domain.Features.Read.ItemSearch;

public sealed class ItemSearchQuery(string itemName) : IQuery<List<ItemSearchResponse>>
{
    public readonly string ItemName = itemName;
}