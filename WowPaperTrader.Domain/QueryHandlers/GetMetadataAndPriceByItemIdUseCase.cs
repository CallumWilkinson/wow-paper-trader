using WowPaperTrader.Domain.Interfaces;
using WowPaperTrader.Domain.ResponseTypes;

namespace WowPaperTrader.Domain.QueryHandlers;

public sealed class GetMetadataAndPriceByItemIdUseCase
{
    private readonly IItemMetadataAndPriceQuery _query;

    public GetMetadataAndPriceByItemIdUseCase(IItemMetadataAndPriceQuery query)
    {
        _query = query;
    }

    public async Task<ItemMetadataAndPriceResult?> ExecuteAsync(
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