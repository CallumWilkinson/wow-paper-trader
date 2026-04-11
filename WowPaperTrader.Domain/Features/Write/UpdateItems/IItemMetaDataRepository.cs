namespace WowPaperTrader.Domain.Features.Write.UpdateItems;

public interface IItemMetaDataRepository
{
    Task SaveItemMetaDataAsync(List<ItemMetaDataRecordResponse> itemMetaDataRecords, CancellationToken cancellationToken);
}