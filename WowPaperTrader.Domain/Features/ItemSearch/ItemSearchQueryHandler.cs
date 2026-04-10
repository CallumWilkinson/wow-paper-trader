namespace WowPaperTrader.Domain.Features.ItemSearch;

public sealed class ItemSearchQueryHandler
{
    private readonly IItemSearchReadService _readService;

    public ItemSearchQueryHandler(IItemSearchReadService readService)
    {
        _readService = readService;
    }

    public async Task<List<ItemSearchResponse>> ExecuteAsync(string itemName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(itemName))
            throw new ArgumentNullException
            (
                nameof(itemName),
                "You must enter an item name"
            );

        var topFiveResults = await _readService.SearchByNameAsync(itemName, cancellationToken);

        return topFiveResults;
    }
}