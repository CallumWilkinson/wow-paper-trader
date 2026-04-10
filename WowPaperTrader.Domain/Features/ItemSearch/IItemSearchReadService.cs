namespace WowPaperTrader.Domain.Features.ItemSearch;

public interface IItemSearchReadService
{
    Task<List<ItemSearchResponse>> SearchByNameAsync(
        string ItemName,
        CancellationToken cancellationToken
    );
}