using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class BattleNetAuthClient
{
    public string? AccessToken { get; private set; }

    public DateTime TokenCreatedAt { get; private set; }

    private readonly string _clientId;

    private readonly string _clientSecret;

    private readonly string TokenUrl = "https://oauth.battle.net/token";

    private readonly HttpClient _httpClient;

    public BattleNetAuthClient(IConfiguration config, HttpClient httpClient)
    {
        _httpClient = httpClient;

        //access dotnet user secrets
        _clientId = config["Blizzard:ClientId"] ?? throw new InvalidOperationException("Blizzard:ClientId is missing");
        _clientSecret = config["Blizzard:ClientSecret"] ?? throw new InvalidOperationException("Blizzard:ClientSecret is missing");
    }

    public async Task<string?> RequestNewTokenAsync(CancellationToken cancellationToken)
    {
        //OAuth 2.0 requires format to be Authorization: Basic base64(client_id:client_secret)
        var basicAuthBytes = Encoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}");
        var authHeaderValue = Convert.ToBase64String(basicAuthBytes);

        using var request = new HttpRequestMessage(HttpMethod.Post, TokenUrl);

        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials"
        });

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        using var doc = JsonDocument.Parse(json);
        var token = doc.RootElement.GetProperty("access_token").GetString();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException("'access_token' was empty.");
        }

        AccessToken = token;

        //next i want to use the actual creation time inside the json response
        TokenCreatedAt = DateTime.UtcNow; ;

        return token;
    }
}