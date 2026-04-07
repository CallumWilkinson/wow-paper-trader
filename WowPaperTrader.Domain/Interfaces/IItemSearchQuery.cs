using WowPaperTrader.Application.Read.Contracts;

namespace WowPaperTrader.Application.Read.Interfaces;

public interface IItemSearchQuery
{
    Task<List<ItemSearchResult>> SearchByNameAsync(
        string ItemName,
        CancellationToken cancellationToken
    );
}