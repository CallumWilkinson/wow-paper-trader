using WowPaperTrader.Application.Architecture;

namespace WowPaperTrader.Application.Features.Read.LowestPrice;

public sealed class LowestPriceQueryHandler(ILowestPriceReadService readService) : IQueryHandler<LowestPriceQuery, LowestPriceResponse>
{
    public async Task<LowestPriceResponse> HandleAsync(LowestPriceQuery query, CancellationToken cancellationToken)
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