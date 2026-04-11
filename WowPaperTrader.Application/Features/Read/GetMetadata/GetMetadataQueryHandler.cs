using WowPaperTrader.Application.Architecture;

namespace WowPaperTrader.Application.Features.Read.GetMetadata;

public sealed class GetMetadataQueryHandler(IMetadataReadService readService)
    : IQueryHandler<GetMetadataQuery, MetadataResponse>
{
    public async Task<MetadataResponse> HandleAsync(GetMetadataQuery query, CancellationToken cancellationToken)
    {
        if (query.ItemId <= 0)
            throw new ArgumentOutOfRangeException
            (
                nameof(query.ItemId),
                "Invalid itemId"
            );

        return await readService.GetAsync(query.ItemId, cancellationToken);
    }

    
}