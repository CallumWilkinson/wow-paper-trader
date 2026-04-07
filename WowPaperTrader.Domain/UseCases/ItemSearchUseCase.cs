using WowPaperTrader.Application.Read.Contracts;
using WowPaperTrader.Application.Read.Interfaces;

namespace WowPaperTrader.Application.Read.UseCases;

public sealed class ItemSearchUseCase
{
    private readonly IItemSearchQuery _query;

    public ItemSearchUseCase(IItemSearchQuery query)
    {
        _query = query;
    }

    public async Task<List<ItemSearchResult>> ExecuteAsync(string itemName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(itemName))
        {
            throw new ArgumentNullException
            (
                nameof(itemName),
                "You must enter an item name"
            );
        }

        var topFiveResults = await _query.SearchByNameAsync(itemName, cancellationToken);

        return topFiveResults;
    }
}