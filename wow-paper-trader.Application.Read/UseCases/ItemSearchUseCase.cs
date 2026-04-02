public sealed class ItemSearchUseCase
{
    private readonly IItemSearchQuery _query;

    public ItemSearchUseCase(IItemSearchQuery query)
    {
        _query = query;
    }

    public async Task<List<ItemSearchResult>> ExecuteAsync(string itemName, CancellationToken cancellationToken)
    {
        string normalisedName = itemName.Trim();

        var items = await _query.SearchByNameAsync(normalisedName, cancellationToken);

        var itemList = items
            .Select(x => new ItemSearchResult
            {
                ItemId = x.ItemId,
                Name = x.Name
            })
            .ToList();

        return itemList;
    }
}