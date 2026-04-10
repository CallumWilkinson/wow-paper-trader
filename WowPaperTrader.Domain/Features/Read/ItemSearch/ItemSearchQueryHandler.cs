using WowPaperTrader.Domain.Architecture;

namespace WowPaperTrader.Domain.Features.Read.ItemSearch;

public sealed class ItemSearchQueryHandler(IItemSearchReadService readService)
    : IQueryHandler<ItemSearchQuery, List<ItemSearchResponse>>
{
    public async Task<List<ItemSearchResponse>> HandleAsync(ItemSearchQuery query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query.ItemName))
            throw new ArgumentNullException
            (
                nameof(query.ItemName),
                "You must enter an item name"
            );

        var topFiveResults = await readService.SearchByNameAsync(query.ItemName, cancellationToken);

        return topFiveResults;
    }
    
}