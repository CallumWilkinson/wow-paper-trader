public interface IItemIdsWithoutMetadataQuery
{
    Task<List<long>> GetItemIdsWithoutMetadataAsync(CancellationToken cancellationToken);
}