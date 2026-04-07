using WowPaperTrader.Application.Read.Contracts;

namespace WowPaperTrader.Application.Read.Interfaces;

public interface ICurrentLowestUnitPriceQuery
{
    Task<CurrentLowestUnitPriceResult?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}