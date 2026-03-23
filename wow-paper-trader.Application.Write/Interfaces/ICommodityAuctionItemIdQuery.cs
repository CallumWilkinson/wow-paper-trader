public interface ICommodityAuctionItemIdQuery
{
    Task<List<long>> GetAllUniqueItemIdsAsync(CancellationToken cancellationToken);
}