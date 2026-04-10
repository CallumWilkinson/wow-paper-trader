namespace WowPaperTrader.Domain.Features.ItemSearch;

public interface IItemSearchQuery
{
    Task<List<ItemSearchResult>> SearchByNameAsync(
        string ItemName,
        CancellationToken cancellationToken
    );
}