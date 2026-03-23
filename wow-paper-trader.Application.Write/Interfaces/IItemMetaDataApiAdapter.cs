public interface IItemMetaDataApiAdapter
{
    Task<ItemMetaDataRecord> GetItemMetaDataAsync(long itemId, CancellationToken cancellationToken);
}