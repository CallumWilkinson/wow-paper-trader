using WowPaperTrader.Domain.Contracts;

namespace WowPaperTrader.Domain.Interfaces;

public interface ICurrentLowestUnitPriceQuery
{
    Task<CurrentLowestUnitPriceResult?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}