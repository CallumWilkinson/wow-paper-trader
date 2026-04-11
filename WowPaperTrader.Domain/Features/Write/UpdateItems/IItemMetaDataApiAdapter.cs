using WowPaperTrader.Domain.ResponseTypes;

namespace WowPaperTrader.Domain.Interfaces;

public interface IItemMetaDataApiAdapter
{
    Task<ItemMetaDataRecordResponse> GetItemMetaDataAsync(long itemId, CancellationToken cancellationToken);
}