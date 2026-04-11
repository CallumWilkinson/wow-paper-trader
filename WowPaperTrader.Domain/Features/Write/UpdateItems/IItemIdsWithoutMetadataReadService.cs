namespace WowPaperTrader.Domain.Features.Write.UpdateItems;

public interface IItemIdsWithoutMetadataReadService
{
    Task<List<long>> GetItemIdsWithoutMetadataAsync(CancellationToken cancellationToken);
}