using System.Data;

public sealed class CommodityAuctionApiAdapter : ICommodityAuctionApiAdapter
{
    private readonly BattleNetAuthClient _authClient;

    private readonly CommodityAuctionClient _auctionClient;

    public CommodityAuctionApiAdapter(BattleNetAuthClient authClient, CommodityAuctionClient auctionClient)
    {
        _authClient = authClient;
        _auctionClient = auctionClient;
    }
    public async Task<WowApiResult<AuctionSnapshot>> GetCommodityAuctionsSnapshotAsync(CancellationToken cancellationToken)
    {
        //add 24hr token refresh logic
        string accessToken = await _authClient.RequestNewTokenAsync(cancellationToken);

        var result = await _auctionClient.GetCommodityAuctionsAsync(accessToken, cancellationToken);

        return result;

    }
}