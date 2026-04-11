using WowPaperTrader.Application.Architecture;

namespace WowPaperTrader.Application.Features.Read.ItemSearch;

public sealed class ItemSearchQuery(string itemName) : IQuery<List<ItemSearchResponse>>
{
    public readonly string ItemName = itemName;
}