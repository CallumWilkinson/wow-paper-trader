using WowPaperTrader.Domain.Contracts;

namespace WowPaperTrader.Domain.Interfaces;

public interface IItemMetaDataRepository
{
    Task SaveItemMetaDataAsync(List<ItemMetaDataRecord> itemMetaDataRecords, CancellationToken cancellationToken);
}