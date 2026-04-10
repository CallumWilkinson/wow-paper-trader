using WowPaperTrader.Domain.Interfaces;
using WowPaperTrader.Domain.ResponseTypes;

namespace WowPaperTrader.Domain.QueryHandlers;

public sealed class GetCurrentLowestUnitPriceByItemIdUseCase
{
    private readonly ICurrentLowestUnitPriceReadService _query;

    public GetCurrentLowestUnitPriceByItemIdUseCase(
        ICurrentLowestUnitPriceReadService query
    )
    {
        _query = query;
    }

    public async Task<CurrentLowestUnitPriceResult?> ExecuteAsync(
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

        return await _query.GetAsync(itemId, cancellationToken);
    }
}