namespace WowPaperTrader.Domain.Features.Read.ItemSearch;

public interface IItemSearchReadService
{
    Task<List<ItemSearchResponse>> SearchByNameAsync(
        string itemName,
        CancellationToken cancellationToken
    );
}