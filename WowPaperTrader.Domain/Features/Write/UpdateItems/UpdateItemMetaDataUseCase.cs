using Microsoft.Extensions.Logging;

namespace WowPaperTrader.Domain.Features.Write.UpdateItems;

public sealed class UpdateItemMetaDataUseCase
{
    private readonly IItemIdsWithoutMetadataReadService _itemIdsWithoutMetadataReadService;
    private readonly IItemMetaDataApiAdapter _itemMetaDataApiAdapter;
    private readonly IItemMetaDataRepository _itemMetaDataRepository;
    private readonly ILogger<UpdateItemMetaDataUseCase> _logger;

    public UpdateItemMetaDataUseCase(
        IItemIdsWithoutMetadataReadService itemIdsWithoutMetadataReadService,
        IItemMetaDataApiAdapter itemMetaDataApiAdapter,
        IItemMetaDataRepository itemMetaDataRepository,
        ILogger<UpdateItemMetaDataUseCase> logger
    )
    {
        _itemIdsWithoutMetadataReadService = itemIdsWithoutMetadataReadService;
        _itemMetaDataApiAdapter = itemMetaDataApiAdapter;
        _itemMetaDataRepository = itemMetaDataRepository;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var itemIds = await _itemIdsWithoutMetadataReadService.GetItemIdsWithoutMetadataAsync(cancellationToken);

            var itemMetaDataRecords = new List<ItemMetaDataRecordResponse>();

            var itemIdsForMetaDataNotFound = new List<long>();

            var itemIdsThatFailedOnHttpError = new List<long>();

            foreach (var itemId in itemIds)
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

            await _itemMetaDataRepository.SaveItemMetaDataAsync(itemMetaDataRecords, cancellationToken);

            _logger.LogInformation(
                "Items that have auctions listed but no meta data from blizzard: {itemIdsForMetaDataNotFound}",
                string.Join(", ", itemIdsForMetaDataNotFound));

            _logger.LogInformation("Items that failled on http request to blizzard: {itemIdsThatFailedOnHttpError}",
                string.Join(", ", itemIdsThatFailedOnHttpError));

            _logger.LogInformation("Update Item MetaData Use Case Completed Successfully");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Update Item MetaData Use Case Cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update Item MetaData Use Case Failed");
            throw;
        }
    }
}