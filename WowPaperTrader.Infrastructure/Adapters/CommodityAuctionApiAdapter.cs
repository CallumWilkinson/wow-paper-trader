using Microsoft.Extensions.Logging;
using WowPaperTrader.Domain.Contracts;
using WowPaperTrader.Domain.Interfaces;
using WowPaperTrader.Infrastructure.ContractMappers;
using WowPaperTrader.Infrastructure.HttpClients;

namespace WowPaperTrader.Infrastructure.Adapters;

public sealed class CommodityAuctionApiAdapter : ICommodityAuctionApiAdapter
{
    private readonly CommodityAuctionClient _auctionClient;
    private readonly BattleNetAuthClient _authClient;

    private readonly ILogger<CommodityAuctionApiAdapter> _logger;

    public CommodityAuctionApiAdapter(BattleNetAuthClient authClient, CommodityAuctionClient auctionClient,
        ILogger<CommodityAuctionApiAdapter> logger)
    {
        _authClient = authClient;
        _auctionClient = auctionClient;
        _logger = logger;
    }

    public async Task<WowApiResult<AuctionSnapshot>> GetCommodityAuctionsSnapshotAsync(
        CancellationToken cancellationToken)
    {
        var accessToken = await _authClient.RequestNewTokenAsync(cancellationToken);

        if (accessToken == null)
            throw new InvalidOperationException("Access token is null. OAuth token acquisition likely failed.");

        var resultWithDto = await _auctionClient.GetCommodityAuctionsAsync(accessToken, cancellationToken);

        var auctionsCount = resultWithDto.Payload.CommodityAuctions.Count;
        _logger.LogInformation("Total Auctions Received: {Count}", auctionsCount);

        var resultWithAuctionSnapshot = WowApiResultMapper.MapToContract(resultWithDto);

        return resultWithAuctionSnapshot;
    }
}