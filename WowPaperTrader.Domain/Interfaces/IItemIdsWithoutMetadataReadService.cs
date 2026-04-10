namespace WowPaperTrader.Domain.Interfaces;

public interface IItemIdsWithoutMetadataReadService
{
    Task<List<long>> GetItemIdsWithoutMetadataAsync(CancellationToken cancellationToken);
}