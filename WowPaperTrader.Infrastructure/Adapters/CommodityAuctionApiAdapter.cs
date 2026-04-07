using Microsoft.Extensions.Logging;
using WowPaperTrader.Infrastructure.ContractMappers;
using WowPaperTrader.Infrastructure.HttpClients;

namespace WowPaperTrader.Infrastructure.Adapters;

public sealed class CommodityAuctionApiAdapter : ICommodityAuctionApiAdapter
{
    private readonly BattleNetAuthClient _authClient;

    private readonly CommodityAuctionClient _auctionClient;

    private readonly ILogger<CommodityAuctionApiAdapter> _logger;

    public CommodityAuctionApiAdapter(BattleNetAuthClient authClient, CommodityAuctionClient auctionClient,
        ILogger<CommodityAuctionApiAdapter> logger)
    {
        _authClient = authClient;
        _auctionClient = auctionClient;
        _logger = logger;
    }
    public async Task<WowApiResult<AuctionSnapshot>> GetCommodityAuctionsSnapshotAsync(CancellationToken cancellationToken)
    {
        string? accessToken = await _authClient.RequestNewTokenAsync(cancellationToken);

        if (accessToken == null)
        {
            throw new InvalidOperationException("Access token is null. OAuth token acquisition likely failed.");
        }

        var resultWithDto = await _auctionClient.GetCommodityAuctionsAsync(accessToken, cancellationToken);

        int auctionsCount = resultWithDto.Payload.CommodityAuctions.Count;
        _logger.LogInformation("Total Auctions Received: {Count}", auctionsCount);

        var resultWithAuctionSnapshot = WowApiResultMapper.MapToContract(resultWithDto);

        return resultWithAuctionSnapshot;

    }


}