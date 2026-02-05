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
    public WowApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T> GetAsync<T>(string requestUri, string accessToken, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"WoW API Request Failed. Status={(int)response.StatusCode} {response.ReasonPhrase}. Body={body}");

        }

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        //convert JSON to C# object, <T> is the DTO
        var result = await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions, cancellationToken)
            ?? throw new JsonException("Wow API response JSON deserialized to null.");

        return result;
    }
}