namespace WowPaperTrader.Domain.Features.Read.LowestPrice;

public interface ICurrentLowestUnitPriceReadService
{
    Task<CurrentLowestUnitPriceResponse?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}