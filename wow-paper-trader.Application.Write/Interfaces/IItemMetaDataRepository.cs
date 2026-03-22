public interface IItemMetaDataRepository
{
    Task SaveMetaDataAsync(List<ItemMetaDataResult> itemMetaDataResults, CancellationToken cancellationToken);
}