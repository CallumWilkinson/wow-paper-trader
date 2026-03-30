using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class ItemMediaClient
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ItemMediaClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ItemMediaResponseDto> GetAsync(string accessToken, long itemId, CancellationToken cancellationToken)
    {
        string endpointSuffix = $"media/item/{itemId}?namespace=static-us&locale=en_US";

        using var request = new HttpRequestMessage(HttpMethod.Get, endpointSuffix);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Wow Api Media request failed during Http call. Status={(int)response.StatusCode} {response.ReasonPhrase}. Body={body}");
        }

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var result = await JsonSerializer.DeserializeAsync<ItemMediaResponseDto>(stream, _jsonOptions, cancellationToken)
        ?? throw new JsonException("Wow api Media response Json derserialised to null");

        return result;
    }
}