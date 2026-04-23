using WowPaperTrader.Application.Architecture;

namespace WowPaperTrader.Application.Features.Read.MonthlyPriceQuantity;

public sealed class MonthlyPriceQuantityQueryHandler(IMonthlyPriceQuantityReadService readService): IQueryHandler<MonthlyPriceQuantityQuery, MonthlyPriceQuantityResponse>
{
    public async Task<MonthlyPriceQuantityResponse> HandleAsync(MonthlyPriceQuantityQuery query, CancellationToken cancellationToken)
    {
        if (query.ItemId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(query.ItemId),
                "Invalid itemId");
        
        return await readService.GetAsync(query.ItemId, cancellationToken);
    }
}