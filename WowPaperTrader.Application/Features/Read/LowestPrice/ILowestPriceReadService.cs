namespace WowPaperTrader.Application.Features.Read.LowestPrice;

public interface ILowestPriceReadService
{
    Task<LowestPriceResponse?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}