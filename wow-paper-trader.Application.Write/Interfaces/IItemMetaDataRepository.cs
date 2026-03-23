public interface IItemMetaDataRepository
{
    Task SaveItemMetaDataAsync(List<ItemMetaDataRecord> itemMetaDataRecords, CancellationToken cancellationToken);
}