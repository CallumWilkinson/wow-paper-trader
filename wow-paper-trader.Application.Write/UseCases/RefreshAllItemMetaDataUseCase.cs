public sealed class RefreshAllItemMetaDataUseCase
{
    private readonly ICommodityAuctionItemIdQuery _commodityAuctionItemIdQuery;
    private readonly IItemMetaDataApiAdapter _itemMetaDataApiAdapter;
    private readonly IItemMetaDataRepository _itemMetaDataRepository;

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

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var itemIds = await _commodityAuctionItemIdQuery.GetAllUniqueItemIdsAsync(cancellationToken);

        var itemMetaDataRecords = new List<ItemMetaDataRecord>();

        foreach (long itemId in itemIds)
        {
            var record = await _itemMetaDataApiAdapter.GetItemMetaDataAsync(itemId, cancellationToken);

            itemMetaDataRecords.Add(record);
        }

        await _itemMetaDataRepository.SaveItemMetaDataAsync(itemMetaDataRecords, cancellationToken);
    }

}