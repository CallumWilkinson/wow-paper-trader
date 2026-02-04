using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class BattleNetAuthClient
{
    public string? AccessToken { get; private set; }

    //every 24 hours we will re-instantiate a new BattleNetAuthClient to get a new token
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

        //add to auth header
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

        using var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials"
        });

        using var response = await _httpClient.PostAsync(TokenUrl, form);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        var token = doc.RootElement.GetProperty("access_token").GetString();

        AccessToken = token;

        var nowUtc = DateTime.UtcNow;
        TokenCreatedAt = nowUtc;

        return token;
    }
}