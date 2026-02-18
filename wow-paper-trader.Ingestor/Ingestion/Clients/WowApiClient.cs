using System.Net.Http.Headers;
using System.Text.Json;

public sealed class WowApiClient
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        //json is camelCase, C# objects are Pascal, this avoids issues when mapping to DTOs
        PropertyNameCaseInsensitive = true
    };

    public sealed record WowApiResult<T>
    (
        T Payload,
        DateTime DataReturnedAtUtc,
        string Endpoint
    );


    public WowApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WowApiResult<CommodityAuctionsResponseDto>> GetCommodityAuctionsAsync(string accessToken, CancellationToken cancellationToken)
    {
        string endpointSuffix = "auctions/commodities?namespace=dynamic-us&locale=en_US";
        using var request = new HttpRequestMessage(HttpMethod.Get, endpointSuffix);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"WoW API Request Failed. Status={(int)response.StatusCode} {response.ReasonPhrase}. Body={body}");

        }

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        //convert JSON to C# object (the dto)
        CommodityAuctionsResponseDto result = await JsonSerializer.DeserializeAsync<CommodityAuctionsResponseDto>(stream, JsonOptions, cancellationToken)
            ?? throw new JsonException("Wow API response JSON deserialized to null.");

        string fullEndpoint = new Uri(_httpClient.BaseAddress!, endpointSuffix).ToString();

        return new WowApiResult<CommodityAuctionsResponseDto>(result, DateTime.UtcNow, fullEndpoint);
    }
}