namespace WowPaperTrader.Domain.Features.Write.UpdateItems;

public interface IItemMetaDataApiAdapter
{
    Task<ItemMetaDataRecord> GetItemMetaDataAsync(long itemId, CancellationToken cancellationToken);
}