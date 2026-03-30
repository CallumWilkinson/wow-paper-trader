public interface IItemMetadataAndPriceQuery
{
    Task<ItemMetadataAndPriceResult?> GetAsync(
        long itemId,
        CancellationToken cancellationToken
    );
}