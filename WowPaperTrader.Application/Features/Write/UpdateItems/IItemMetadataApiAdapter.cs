namespace WowPaperTrader.Application.Features.Write.UpdateItems;

public interface IItemMetadataApiAdapter
{
    Task<ItemMetadataRecord> GetItemMetaDataAsync(long itemId, CancellationToken cancellationToken);
}