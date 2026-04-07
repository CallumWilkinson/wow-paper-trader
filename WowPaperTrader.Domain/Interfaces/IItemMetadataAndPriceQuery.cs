using WowPaperTrader.Application.Read.Contracts;

namespace WowPaperTrader.Application.Read.Interfaces;

public interface IItemMetadataAndPriceQuery
{
    Task<ItemMetadataAndPriceResult?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}