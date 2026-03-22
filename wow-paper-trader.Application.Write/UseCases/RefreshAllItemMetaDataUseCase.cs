public sealed class RefreshAllItemMetaDataUseCase
{
    ICommodityAuctionItemIdQuery _commodityAuctionItemIdQuery;
    IItemMetaDataApiAdapter _itemMetaDataApiAdapter;
    IItemMetaDataRepository _itemMetaDataRepository;

    public RefreshAllItemMetaDataUseCase(
        ICommodityAuctionItemIdQuery commodityAuctionItemIdQuery,
        IItemMetaDataApiAdapter itemMetaDataApiAdapter,
        IItemMetaDataRepository itemMetaDataRepository
    )
    {
        _commodityAuctionItemIdQuery = commodityAuctionItemIdQuery;
        _itemMetaDataApiAdapter = itemMetaDataApiAdapter;
        _itemMetaDataRepository = itemMetaDataRepository;
    }

    public async void ExecuteAsync(CancellationToken cancellationToken)
    {
        List<long> itemIds = await _commodityAuctionItemIdQuery.GetAllUniqueIdsAsync(cancellationToken);

        var itemMetaDataResults = new List<ItemMetaDataResult>();

        foreach (long itemId in itemIds)
        {
            var result = await _itemMetaDataApiAdapter.GetItemMetaDataAsync(itemId);

            itemMetaDataResults.Add(result);
        }

        await _itemMetaDataRepository.SaveMetaDataAsync(itemMetaDataResults, cancellationToken);
    }

}