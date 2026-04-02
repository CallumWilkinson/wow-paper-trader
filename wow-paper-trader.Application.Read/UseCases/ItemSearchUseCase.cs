public sealed class ItemSearchUseCase
{
    private readonly IItemSearchQuery _query;

    public ItemSearchUseCase(IItemSearchQuery query)
    {
        _query = query;
    }

    public async Task<List<ItemSearchResult>> ExecuteAsync(string itemName, CancellationToken cancellationToken)
    {
        //TODO: add null check

        string normalisedName = itemName.Trim();

        var candidates = await _query.SearchCandidatesByNameAsync(itemName, cancellationToken);

        var rankedResults = candidates
            .OrderBy(x => GetMatchRank(x.Name, itemName))
            .ThenBy(x => x.Name.Length)
            .ThenBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .Take(5)
            .ToList();

        return rankedResults;
    }

    private static int GetMatchRank(string candidateName, string searchText)
    {
        if (candidateName.Equals(searchText, StringComparison.OrdinalIgnoreCase))
        {
            return 1;
        }

        if (candidateName.StartsWith(searchText, StringComparison.OrdinalIgnoreCase))
        {
            return 2;
        }

        if (candidateName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
        {
            return 3;
        }

        return 4;
    }
}