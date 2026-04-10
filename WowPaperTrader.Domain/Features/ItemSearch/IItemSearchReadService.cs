namespace WowPaperTrader.Domain.Features.ItemSearch;

public interface IItemSearchReadService
{
    Task<List<ItemSearchResult>> SearchByNameAsync(
        string ItemName,
        CancellationToken cancellationToken
    );
}