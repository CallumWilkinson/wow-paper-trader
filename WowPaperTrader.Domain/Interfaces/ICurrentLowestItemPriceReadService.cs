using WowPaperTrader.Domain.ResponseTypes;

namespace WowPaperTrader.Domain.Interfaces;

public interface ICurrentLowestUnitPriceReadService
{
    Task<CurrentLowestUnitPriceResult?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}