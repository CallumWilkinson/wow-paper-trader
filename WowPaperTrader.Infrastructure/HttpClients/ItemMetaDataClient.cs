using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using WowPaperTrader.Infrastructure.DTOs;

namespace WowPaperTrader.Infrastructure.HttpClients;

public sealed class ItemMetaDataClient
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;

    public ItemMetaDataClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ItemMetaDataResponseDto> GetAsync(string accessToken, long itemId,
        CancellationToken cancellationToken)
    {
        var endpointSuffix = $"item/{itemId}?namespace=static-us&locale=en_US";

        using var request = new HttpRequestMessage(HttpMethod.Get, endpointSuffix);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(
                $"WoW API ItemMetaData Request Failed during HTTP call. Status={(int)response.StatusCode} {response.ReasonPhrase}. Body={body}");
        }

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var result =
            await JsonSerializer.DeserializeAsync<ItemMetaDataResponseDto>(stream, _jsonOptions, cancellationToken)
            ?? throw new JsonException("Wow Api ItemMetaData response JSON deserialised to null.");

        return result;
    }
}