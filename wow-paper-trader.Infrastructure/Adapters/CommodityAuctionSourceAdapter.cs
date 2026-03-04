
public sealed class CommodityAuctionSourceAdapter : ICommodityAuctionAdapter
{
    private readonly BattleNetAuthClient _authClient;

    private readonly CommodityAuctionClient _auctionClient;

    public CommodityAuctionSourceAdapter(BattleNetAuthClient authClient, CommodityAuctionClient auctionClient)
    {
        _authClient = authClient;
        _auctionClient = auctionClient;
    }
    public async Task<CommodityAuctionSnapshotResult> GetCommodityAuctionsAsync(CancellationToken cancellationToken)
    {
        string accessToken = await _authClient.
    }
}


// not using an interface for the auth just passing it directly

// // wow-paper-trader.Infrastructure/Ingestion/CommodityAuctionSource.cs
// using wow_paper_trader.Application.Write.Ingestion;
// using wow_paper_trader.Infrastructure.Auth;
// using wow_paper_trader.Infrastructure.Http;

// namespace wow_paper_trader.Infrastructure.Ingestion;

// public sealed class CommodityAuctionSource : ICommodityAuctionSource
// {
//     private readonly IBattleNetAccessTokenProvider _tokenProvider;
//     private readonly CommodityAuctionClient _client;

//     public CommodityAuctionSource(
//         IBattleNetAccessTokenProvider tokenProvider,
//         CommodityAuctionClient client)
//     {
//         _tokenProvider = tokenProvider;
//         _client = client;
//     }

//     public async Task<CommodityAuctionSnapshot> GetCommodityAuctionsAsync(CancellationToken cancellationToken)
//     {
//         string accessToken = await _tokenProvider.GetAccessTokenAsync(cancellationToken);

//         CommodityAuctionClient.WowApiResult<CommodityAuctionsResponseDto> result =
//             await _client.GetCommodityAuctionsAsync(accessToken, cancellationToken);

//         return Map(result);
//     }

//     private static CommodityAuctionSnapshot Map(CommodityAuctionClient.WowApiResult<CommodityAuctionsResponseDto> result)
//     {
//         // Adjust property names to match your real DTO shape.
//         var auctions = result.Payload.CommodityAuctions
//             .Select(a => new CommodityAuctionRow(
//                 AuctionId: a.Id,
//                 ItemId: a.Item.Id,
//                 Quantity: a.Quantity,
//                 UnitPrice: a.UnitPrice,
//                 TimeLeft: a.TimeLeft))
//             .ToList();

//         return new CommodityAuctionSnapshot(
//             DataReturnedAtUtc: result.DataReturnedAtUtc,
//             Endpoint: result.Endpoint,
//             Auctions: auctions);
//     }
// }