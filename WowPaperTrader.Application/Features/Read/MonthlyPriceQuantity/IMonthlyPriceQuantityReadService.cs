namespace WowPaperTrader.Application.Features.Read.MonthlyPriceQuantity;

public interface IMonthlyPriceQuantityReadService
{
    Task<MonthlyPriceQuantityResponse> GetAsync(long itemId, CancellationToken cancellationToken);
}