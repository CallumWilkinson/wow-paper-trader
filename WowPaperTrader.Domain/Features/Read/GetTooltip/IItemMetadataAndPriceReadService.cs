namespace WowPaperTrader.Domain.Features.Read.GetTooltip;

public interface IItemMetadataAndPriceReadService
{
    Task<TooltipResponse?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}