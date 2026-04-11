using Microsoft.Extensions.Logging;
using WowPaperTrader.Application.Features.Write.UpdateItems;
using WowPaperTrader.Infrastructure.ContractMappers;
using WowPaperTrader.Infrastructure.HttpClients;

namespace WowPaperTrader.Infrastructure.Adapters;

public sealed class ItemMetadataApiAdapter : IItemMetadataApiAdapter
{
    private readonly BattleNetAuthClient _authClient;
    private readonly ItemMediaClient _itemMediaClient;
    private readonly ItemMetaDataClient _itemMetaDataClient;
    private readonly ILogger<ItemMetadataApiAdapter> _logger;

    public ItemMetadataApiAdapter(
        BattleNetAuthClient authClient,
        ItemMetaDataClient itemMetaDataClient,
        ItemMediaClient itemMediaClient,
        ILogger<ItemMetadataApiAdapter> logger)
    {
        _authClient = authClient;
        _itemMetaDataClient = itemMetaDataClient;
        _itemMediaClient = itemMediaClient;
        _logger = logger;
    }

    public async Task<ItemMetadataRecord> GetItemMetaDataAsync(long itemId, CancellationToken cancellationToken)
    {
        var accessToken = await _authClient.RequestNewTokenAsync(cancellationToken);

        if (accessToken == null)
            throw new InvalidOperationException("Access token is null. OAuth token acquisition likely failed.");

        var metadataDto = await _itemMetaDataClient.GetAsync(accessToken, itemId, cancellationToken);

        var mediaDto = await _itemMediaClient.GetAsync(accessToken, itemId, cancellationToken);

        if (metadataDto == null || mediaDto == null) return null;

        var dataFetchedAtUtc = DateTime.UtcNow;

        var contract = ItemMetaDataRecordMapper.MapToContract(metadataDto, mediaDto, dataFetchedAtUtc);

        return contract;
    }
}