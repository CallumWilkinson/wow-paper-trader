using WowPaperTrader.Application.Read.Contracts;

namespace WowPaperTrader.Application.Read.Interfaces;

public interface IItemMetaDataApiAdapter
{
    Task<ItemMetaDataRecord> GetItemMetaDataAsync(long itemId, CancellationToken cancellationToken);
}