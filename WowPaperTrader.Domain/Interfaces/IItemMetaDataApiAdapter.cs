using WowPaperTrader.Domain.Contracts;

namespace WowPaperTrader.Domain.Interfaces;

public interface IItemMetaDataApiAdapter
{
    Task<ItemMetaDataRecord> GetItemMetaDataAsync(long itemId, CancellationToken cancellationToken);
}