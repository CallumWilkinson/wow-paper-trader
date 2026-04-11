using WowPaperTrader.Domain.Architecture;

namespace WowPaperTrader.Domain.Features.Read.GetMetadata;

public sealed class GetMetadataQueryHandler :  IQueryHandler<GetMetadataQuery, MetadataResponse>
{
    private readonly IMetadataReadService _readService;

    public GetMetadataQueryHandler(IMetadataReadService readService)
    {
        _readService = readService;
    }

    public async Task<MetadataResponse> HandleAsync(GetMetadataQuery query, CancellationToken cancellationToken)
    {
        if (query.ItemId <= 0)
            throw new ArgumentOutOfRangeException
            (
                nameof(query.ItemId),
                "Invalid itemId"
            );

        return await _readService.GetAsync(query.ItemId, cancellationToken);
    }

    
}