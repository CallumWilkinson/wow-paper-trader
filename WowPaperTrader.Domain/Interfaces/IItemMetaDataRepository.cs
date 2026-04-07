using WowPaperTrader.Application.Read.Contracts;

namespace WowPaperTrader.Application.Read.Interfaces;

public interface IItemMetaDataRepository
{
    Task SaveItemMetaDataAsync(List<ItemMetaDataRecord> itemMetaDataRecords, CancellationToken cancellationToken);
}