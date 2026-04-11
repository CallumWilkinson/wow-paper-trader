namespace WowPaperTrader.Domain.Features.Read.GetMetadata;

public sealed class GetMetadataQueryHandler
{
    private readonly IMetadataReadService _readService;

    public GetMetadataQueryHandler(IMetadataReadService readService)
    {
        _readService = readService;
    }

    public async Task<MetadataResponse?> ExecuteAsync(
        long itemId,
        CancellationToken cancellationToken
    )
    {
        if (itemId <= 0)
            throw new ArgumentOutOfRangeException
            (
                nameof(itemId),
                "Invalid itemId"
            );

        return await _readService.GetAsync(itemId, cancellationToken);
    }
}