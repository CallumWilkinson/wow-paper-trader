namespace WowPaperTrader.Domain.Features.Write.UpdateItems;

public interface IItemMetaDataApiAdapter
{
    Task<ItemMetaDataRecordResponse> GetItemMetaDataAsync(long itemId, CancellationToken cancellationToken);
}