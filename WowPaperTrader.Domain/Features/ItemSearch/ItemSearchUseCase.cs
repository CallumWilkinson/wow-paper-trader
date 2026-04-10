namespace WowPaperTrader.Domain.Features.ItemSearch;

public sealed class ItemSearchUseCase
{
    private readonly IItemSearchReadService _readService;

    public ItemSearchUseCase(IItemSearchReadService readService)
    {
        _readService = readService;
    }

    public async Task<List<ItemSearchResult>> ExecuteAsync(string itemName, CancellationToken cancellationToken)
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