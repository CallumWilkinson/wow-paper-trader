namespace WowPaperTrader.Domain.Features.Read.GetTooltip;

public sealed class GetMetadataAndPriceByItemIdUseCase
{
    private readonly IItemMetadataAndPriceReadService _readService;

    public GetMetadataAndPriceByItemIdUseCase(IItemMetadataAndPriceReadService readService)
    {
        _readService = readService;
    }

    public async Task<ItemMetadataAndPriceResponse?> ExecuteAsync(
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