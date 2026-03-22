public interface IItemMetaDataApiAdapter
{
    Task<ItemMetaDataResult> GetItemMetaDataAsync(long itemId, CancellationToken cancellationToken);
}