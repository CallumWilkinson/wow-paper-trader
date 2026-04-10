using WowPaperTrader.Domain.Contracts;
using WowPaperTrader.Domain.Interfaces;

namespace WowPaperTrader.Domain.QueryHandlers;

public sealed class GetCurrentLowestUnitPriceByItemIdUseCase
{
    private readonly ICurrentLowestUnitPriceQuery _query;

    public GetCurrentLowestUnitPriceByItemIdUseCase(
        ICurrentLowestUnitPriceQuery query
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