using WowPaperTrader.Domain.ResponseTypes;

namespace WowPaperTrader.Domain.Interfaces;

public interface IItemMetadataAndPriceReadService
{
    Task<ItemMetadataAndPriceResponse?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}