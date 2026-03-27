using Microsoft.Extensions.Logging;

public sealed class ItemMetaDataApiAdapter : IItemMetaDataApiAdapter
{
    private readonly BattleNetAuthClient _authClient;
    private readonly ItemMetaDataClient _itemMetaDataClient;
    private readonly ILogger<ItemMetaDataApiAdapter> _logger;

    public ItemMetaDataApiAdapter(BattleNetAuthClient authClient, ItemMetaDataClient itemMetaDataClient, ILogger<ItemMetaDataApiAdapter> logger)
    {
        _authClient = authClient;
        _itemMetaDataClient = itemMetaDataClient;
        _logger = logger;
    }
    public async Task<ItemMetaDataRecord> GetItemMetaDataAsync(long itemId, CancellationToken cancellationToken)
    {
        string? accessToken = await _authClient.RequestNewTokenAsync(cancellationToken);

        if (accessToken == null)
        {
            throw new InvalidOperationException("Access token is null. OAuth token acquisition likely failed.");
        }

        var dto = await _itemMetaDataClient.GetAsync(accessToken, itemId, cancellationToken);

        if (dto == null)
        {
            return null;
        }

        var dataFetchedAtUtc = DateTime.UtcNow;

        var contract = ItemMetaDataRecordMapper.MapToContract(dto, dataFetchedAtUtc);

        return contract;

    }
}