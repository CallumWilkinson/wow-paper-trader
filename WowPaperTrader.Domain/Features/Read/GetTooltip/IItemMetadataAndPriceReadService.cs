namespace WowPaperTrader.Domain.Features.Read.GetTooltip;

public interface IItemMetadataAndPriceReadService
{
    Task<ItemMetadataAndPriceResponse?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}