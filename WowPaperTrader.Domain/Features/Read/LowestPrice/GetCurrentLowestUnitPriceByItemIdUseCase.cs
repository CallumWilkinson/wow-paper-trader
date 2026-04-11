namespace WowPaperTrader.Domain.Features.Read.LowestPrice;

public sealed class GetCurrentLowestUnitPriceByItemIdUseCase(ICurrentLowestUnitPriceReadService query)
{
    public async Task<CurrentLowestUnitPriceResponse?> ExecuteAsync(
        long itemId,
        CancellationToken cancellationToken
    )
    {
        if (itemId <= 0)
            throw new ArgumentOutOfRangeException
            (
                nameof(itemId),
                "Invalid itemId"
            );

        return await query.GetAsync(itemId, cancellationToken);
    }
}