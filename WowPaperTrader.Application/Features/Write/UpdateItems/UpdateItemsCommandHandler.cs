using Microsoft.Extensions.Logging;
using WowPaperTrader.Application.Architecture;

namespace WowPaperTrader.Application.Features.Write.UpdateItems;

public sealed class UpdateItemsCommandHandler(
    IItemIdsWithoutMetadataReadService itemIdsWithoutMetadataReadService,
    IItemMetadataApiAdapter itemMetadataApiAdapter,
    IItemMetadataRepository itemMetadataRepository,
    ILogger<UpdateItemsCommandHandler> logger) : ICommandHandler<UpdateItemsCommand>
{
    public async Task HandleAsync(UpdateItemsCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var itemIds = await itemIdsWithoutMetadataReadService.GetItemIdsWithoutMetadataAsync(cancellationToken);

            var itemMetaDataRecords = new List<ItemMetadataRecord>();

            var itemIdsForMetaDataNotFound = new List<long>();

            var itemIdsThatFailedOnHttpError = new List<long>();

            foreach (var itemId in itemIds)
                try
                {
                    var record = await itemMetadataApiAdapter.GetItemMetaDataAsync(itemId, cancellationToken);

                    if (record == null)
                    {
                        logger.LogWarning("Item metadata not found for item {ItemId}. Skipping.", itemId);
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

                    logger.LogWarning(ex, "HTTP failure while fetching metadata for item {ItemId}. Skipping.", itemId);
                }

            await itemMetadataRepository.SaveItemMetaDataAsync(itemMetaDataRecords, cancellationToken);

            logger.LogInformation(
                "Items that have auctions listed but no meta data from blizzard: {itemIdsForMetaDataNotFound}",
                string.Join(", ", itemIdsForMetaDataNotFound));

            logger.LogInformation("Items that failled on http request to blizzard: {itemIdsThatFailedOnHttpError}",
                string.Join(", ", itemIdsThatFailedOnHttpError));

            logger.LogInformation("Update Item MetaData Use Case Completed Successfully");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("Update Item MetaData Use Case Cancelled");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Update Item MetaData Use Case Failed");
            throw;
        }
    }
}