namespace WowPaperTrader.Domain.Features.Read.LowestPrice;

public interface ILowestPriceReadService
{
    Task<LowestPriceResponse?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}