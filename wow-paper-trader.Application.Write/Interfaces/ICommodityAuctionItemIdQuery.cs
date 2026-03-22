public interface ICommodityAuctionItemIdQuery
{
    Task<List<long>> GetAllUniqueIdsAsync(CancellationToken cancellationToken);
}