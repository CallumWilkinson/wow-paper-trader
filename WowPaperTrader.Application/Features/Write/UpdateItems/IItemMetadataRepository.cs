namespace WowPaperTrader.Application.Features.Write.UpdateItems;

public interface IItemMetadataRepository
{
    Task SaveItemMetaDataAsync(List<ItemMetadataRecord> itemMetaDataRecords, CancellationToken cancellationToken);
}