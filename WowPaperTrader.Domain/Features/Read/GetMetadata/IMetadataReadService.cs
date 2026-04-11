namespace WowPaperTrader.Domain.Features.Read.GetMetadata;

public interface IMetadataReadService
{
    Task<MetadataResponse?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}