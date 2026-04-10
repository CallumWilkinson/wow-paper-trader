using System.Net.Http.Headers;
using System.Text.Json;
using WowPaperTrader.Domain.ResponseTypes;
using WowPaperTrader.Infrastructure.DTOs;

namespace WowPaperTrader.Infrastructure.HttpClients;

public sealed class CommodityAuctionClient
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        //json is camelCase, C# objects are Pascal, this avoids issues when mapping to DTOs
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;


    public CommodityAuctionClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WowApiResult<CommodityAuctionsResponseDto>> GetCommodityAuctionsAsync(string accessToken,
        CancellationToken cancellationToken)
    {
        var endpointSuffix = "auctions/commodities?namespace=dynamic-us&locale=en_US";
        using var request = new HttpRequestMessage(HttpMethod.Get, endpointSuffix);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(
                $"WoW API Request Failed. Status={(int)response.StatusCode} {response.ReasonPhrase}. Body={body}");
        }

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        //convert JSON to C# object (the dto)
        var result =
            await JsonSerializer.DeserializeAsync<CommodityAuctionsResponseDto>(stream, _jsonOptions, cancellationToken)
            ?? throw new JsonException("Wow API response JSON deserialized to null.");

        var fullEndpoint = new Uri(_httpClient.BaseAddress!, endpointSuffix).ToString();

        return new WowApiResult<CommodityAuctionsResponseDto>(result, DateTime.UtcNow, fullEndpoint);
    }
}