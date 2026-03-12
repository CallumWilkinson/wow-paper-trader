public interface ICurrentLowestUnitPriceQuery
{
    Task<CurrentLowestUnitPriceResult?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}