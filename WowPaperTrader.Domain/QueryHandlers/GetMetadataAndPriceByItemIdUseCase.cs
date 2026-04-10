using WowPaperTrader.Domain.Interfaces;
using WowPaperTrader.Domain.ResponseTypes;

namespace WowPaperTrader.Domain.QueryHandlers;

public sealed class GetMetadataAndPriceByItemIdUseCase
{
    private readonly IItemMetadataAndPriceReadService _readService;

    public GetMetadataAndPriceByItemIdUseCase(IItemMetadataAndPriceReadService readService)
    {
        _readService = readService;
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

        return await _readService.GetAsync(itemId, cancellationToken);
    }
}