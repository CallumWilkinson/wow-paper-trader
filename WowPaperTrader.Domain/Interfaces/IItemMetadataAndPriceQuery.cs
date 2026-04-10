using WowPaperTrader.Domain.ResponseTypes;

namespace WowPaperTrader.Domain.Interfaces;

public interface IItemMetadataAndPriceQuery
{
    Task<ItemMetadataAndPriceResult?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}