using WowPaperTrader.Domain.ResponseTypes;

namespace WowPaperTrader.Domain.Interfaces;

public interface ICurrentLowestUnitPriceReadService
{
    Task<CurrentLowestUnitPriceResponse?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}