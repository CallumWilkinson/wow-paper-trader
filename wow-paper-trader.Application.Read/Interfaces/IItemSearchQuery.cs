public interface IItemSearchQuery
{
    Task<List<ItemSearchResult>> SearchCandidatesByNameAsync(
        string ItemName,
        CancellationToken cancellationToken
    );
}