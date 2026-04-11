namespace WowPaperTrader.Application.Features.Read.GetMetadata;

public interface IMetadataReadService
{
    Task<MetadataResponse?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}