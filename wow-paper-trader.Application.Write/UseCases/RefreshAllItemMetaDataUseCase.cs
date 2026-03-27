using Microsoft.Extensions.Logging;

public sealed class RefreshAllItemMetaDataUseCase
{
    private readonly ICommodityAuctionItemIdQuery _commodityAuctionItemIdQuery;
    private readonly IItemMetaDataApiAdapter _itemMetaDataApiAdapter;
    private readonly IItemMetaDataRepository _itemMetaDataRepository;
    private readonly ILogger<RefreshAllItemMetaDataUseCase> _logger;

    public RefreshAllItemMetaDataUseCase(
        ICommodityAuctionItemIdQuery commodityAuctionItemIdQuery,
        IItemMetaDataApiAdapter itemMetaDataApiAdapter,
        IItemMetaDataRepository itemMetaDataRepository,
        ILogger<RefreshAllItemMetaDataUseCase> logger
    )
    {
        _commodityAuctionItemIdQuery = commodityAuctionItemIdQuery;
        _itemMetaDataApiAdapter = itemMetaDataApiAdapter;
        _itemMetaDataRepository = itemMetaDataRepository;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var itemIds = await _commodityAuctionItemIdQuery.GetAllUniqueItemIdsAsync(cancellationToken);

            var itemMetaDataRecords = new List<ItemMetaDataRecord>();

            var itemIdsForMetaDataNotFound = new List<long>();

            var itemIdsThatFailedOnHttpError = new List<long>();

            foreach (long itemId in itemIds)
            {
                try
                {
                    var record = await _itemMetaDataApiAdapter.GetItemMetaDataAsync(itemId, cancellationToken);

                    if (record == null)
                    {
                        _logger.LogWarning("Item metadata not found for item {ItemId}. Skipping.", itemId);
                        itemIdsForMetaDataNotFound.Add(itemId);
                        continue;
                    }

                    itemMetaDataRecords.Add(record);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (HttpRequestException ex)
                {
                    itemIdsThatFailedOnHttpError.Add(itemId);

                    _logger.LogWarning(ex, "HTTP failure while fetching metadata for item {ItemId}. Skipping.", itemId);
                }

            }

            await _itemMetaDataRepository.SaveItemMetaDataAsync(itemMetaDataRecords, cancellationToken);

            _logger.LogInformation("Items that have auctions listed but no meta data from blizzard: {itemIdsForMetaDataNotFound}", string.Join(", ", itemIdsForMetaDataNotFound));

            _logger.LogInformation("Items that failled on http request to blizzard: {itemIdsThatFailedOnHttpError}", string.Join(", ", itemIdsThatFailedOnHttpError));

            _logger.LogInformation("Refresh All Item MetaData Use Case Completed Successfully");

        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Refresh All Item MetaData Use Case Cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Refresh All Item MetaData Use Case Failed");
            throw;
        }

    }

}