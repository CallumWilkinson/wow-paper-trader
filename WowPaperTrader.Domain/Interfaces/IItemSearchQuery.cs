using WowPaperTrader.Domain.Contracts;

namespace WowPaperTrader.Domain.Interfaces;

public interface IItemSearchQuery
{
    Task<List<ItemSearchResult>> SearchByNameAsync(
        string ItemName,
        CancellationToken cancellationToken
    );
}