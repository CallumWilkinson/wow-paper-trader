using WowPaperTrader.Application.Read.Contracts;
using WowPaperTrader.Application.Read.Interfaces;

namespace WowPaperTrader.Application.Read.UseCases;

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
        {
            throw new ArgumentOutOfRangeException
            (
                nameof(itemId),
                "Invalid itemId"
            );
        }

        return await _query.GetAsync(itemId, cancellationToken);
    }
}