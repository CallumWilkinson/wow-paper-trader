public sealed class CurrentLowestUnitPriceQuery : ICurrentLowestUnitPriceQuery
{
    public Task<CurrentLowestUnitPriceResult?> GetAsync(long itemId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}