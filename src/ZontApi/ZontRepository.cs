using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi;

/// <summary>
/// Репозиторий доступа к WEB API Zont.
/// </summary>
public class ZontRepository
{
    private static HttpClient s_httpClient = null!;
    private readonly AppSettings _appSettings;

    public ZontRepository(IOptionsSnapshot<AppSettings> appSettings, IHttpClientFactory httpClientFactory)
    {
        s_httpClient = httpClientFactory.CreateClient(AppSettings.ZontHttpClientName);
        _appSettings = appSettings.Value;
    }

    public async Task AuthorizeAsync()
    {
        var requestBody = new { client_name = AppSettings.AppName };

        var response = await s_httpClient.PostAsJsonAsync($"{_appSettings.ZontSettings.ApiUrl}authtokens", requestBody);
        response.EnsureSuccessStatusCode();

        var authResponse =
            await response.Content.ReadFromJsonAsync<ZontAuthResponse>()
            ?? throw new InvalidOperationException("Токен не был получен.");

        var token = authResponse.Token;

        s_httpClient.DefaultRequestHeaders.Add("X-ZONT-Token", $"{token}");
    }

    public async IAsyncEnumerable<string> GetTokensAsync()
    {
        var response = await s_httpClient.GetAsync($"{_appSettings.ZontSettings.ApiUrl}authtokens");
        response.EnsureSuccessStatusCode();

        var tokens =
            await response.Content.ReadFromJsonAsync<ZontTokensResponse>()
            ?? throw new InvalidOperationException("Токены не были получены.");

        foreach (var token in tokens.AuthTokens)
        {
            yield return $"TokenId: {token.TokenId}, Created: {token.Created}, LastUsed: {token.LastUsed}, ClientName: {token.ClientName}";
        }
    }
}

internal record ZontAuthResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("token")] string Token,
    [property: JsonPropertyName("token_id")] string TokenId);

internal record ZontTokensResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("auth_tokens")] ZontTokensAuthTokensResponse[] AuthTokens);

internal record ZontTokensAuthTokensResponse(
    [property: JsonPropertyName("token_id")]
    string TokenId,
    [property: JsonPropertyName("created")]
    [property: JsonConverter(typeof(UnixDateTimeConverter))]
    DateTime Created,
    [property: JsonPropertyName("last_used")]
    [property: JsonConverter(typeof(UnixDateTimeConverter))]
    DateTime LastUsed,
    [property: JsonPropertyName("client_name")]
    string ClientName,
    [property: JsonPropertyName("cookie")]
    string? Cookie,
    [property: JsonPropertyName("expires_at")]
    [property: JsonConverter(typeof(UnixNullableDateTimeConverter))]
    DateTime? ExpiresAt);
