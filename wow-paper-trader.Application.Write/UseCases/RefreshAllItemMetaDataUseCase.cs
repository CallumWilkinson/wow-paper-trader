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

            foreach (long itemId in itemIds)
            {
                var record = await _itemMetaDataApiAdapter.GetItemMetaDataAsync(itemId, cancellationToken);

                itemMetaDataRecords.Add(record);
            }

            await _itemMetaDataRepository.SaveItemMetaDataAsync(itemMetaDataRecords, cancellationToken);

            _logger.LogInformation("Refresh All Item MetaData Use Case Completed Successfully");

        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Refresh All Item MetaData Use Case Cancelled");
        }
        catch
        {
            _logger.LogInformation("Refresh All Item MetaData Use Case Failed");
        }

    }

}