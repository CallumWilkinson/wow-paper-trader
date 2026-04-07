using Microsoft.Extensions.Logging;
using WowPaperTrader.Application.Read.Interfaces;
using WowPaperTrader.Infrastructure.ContractMappers;
using WowPaperTrader.Infrastructure.HttpClients;

namespace WowPaperTrader.Infrastructure.Adapters;

public sealed class ItemMetaDataApiAdapter : IItemMetaDataApiAdapter
{
    private readonly BattleNetAuthClient _authClient;
    private readonly ItemMetaDataClient _itemMetaDataClient;
    private readonly ItemMediaClient _itemMediaClient;
    private readonly ILogger<ItemMetaDataApiAdapter> _logger;

    public ItemMetaDataApiAdapter(
        BattleNetAuthClient authClient,
        ItemMetaDataClient itemMetaDataClient,
        ItemMediaClient itemMediaClient,
        ILogger<ItemMetaDataApiAdapter> logger)
    {
        _authClient = authClient;
        _itemMetaDataClient = itemMetaDataClient;
        _itemMediaClient = itemMediaClient;
        _logger = logger;
    }
    public async Task<ItemMetaDataRecord> GetItemMetaDataAsync(long itemId, CancellationToken cancellationToken)
    {
        string? accessToken = await _authClient.RequestNewTokenAsync(cancellationToken);

        if (accessToken == null)
        {
            throw new InvalidOperationException("Access token is null. OAuth token acquisition likely failed.");
        }

        var metadataDto = await _itemMetaDataClient.GetAsync(accessToken, itemId, cancellationToken);

        var mediaDto = await _itemMediaClient.GetAsync(accessToken, itemId, cancellationToken);

        if (metadataDto == null || mediaDto == null)
        {
            return null;
        }

        var dataFetchedAtUtc = DateTime.UtcNow;

        var contract = ItemMetaDataRecordMapper.MapToContract(metadataDto, mediaDto, dataFetchedAtUtc);

        return contract;

    }
}