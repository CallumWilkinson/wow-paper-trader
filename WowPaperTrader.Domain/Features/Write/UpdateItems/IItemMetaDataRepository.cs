using WowPaperTrader.Domain.ResponseTypes;

namespace WowPaperTrader.Domain.Interfaces;

public interface IItemMetaDataRepository
{
    Task SaveItemMetaDataAsync(List<ItemMetaDataRecordResponse> itemMetaDataRecords, CancellationToken cancellationToken);
}