namespace WowPaperTrader.Application.Read.Interfaces;

public interface IItemIdsWithoutMetadataQuery
{
    Task<List<long>> GetItemIdsWithoutMetadataAsync(CancellationToken cancellationToken);
}