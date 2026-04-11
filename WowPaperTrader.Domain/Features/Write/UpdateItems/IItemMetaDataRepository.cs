namespace WowPaperTrader.Domain.Features.Write.UpdateItems;

public interface IItemMetaDataRepository
{
    Task SaveItemMetaDataAsync(List<ItemMetaDataRecord> itemMetaDataRecords, CancellationToken cancellationToken);
}