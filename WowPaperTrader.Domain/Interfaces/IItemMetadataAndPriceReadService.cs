using WowPaperTrader.Domain.ResponseTypes;

namespace WowPaperTrader.Domain.Interfaces;

public interface IItemMetadataAndPriceReadService
{
    Task<ItemMetadataAndPriceResult?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}