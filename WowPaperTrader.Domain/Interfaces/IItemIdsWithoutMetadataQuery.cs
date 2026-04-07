namespace WowPaperTrader.Domain.Interfaces;

public interface IItemIdsWithoutMetadataQuery
{
    Task<List<long>> GetItemIdsWithoutMetadataAsync(CancellationToken cancellationToken);
}