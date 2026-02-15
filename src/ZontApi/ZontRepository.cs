using CottageSensorAggregator.ZontApi.Auth;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

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
            await response.Content.ReadFromJsonAsync<AuthResponse>()
            ?? throw new InvalidOperationException("Токен не был получен.");

        var token = authResponse.Token;

        s_httpClient.DefaultRequestHeaders.Add("X-ZONT-Token", $"{token}");
    }

    public async IAsyncEnumerable<string> GetTokensAsync()
    {
        var response = await s_httpClient.GetAsync($"{_appSettings.ZontSettings.ApiUrl}authtokens");
        response.EnsureSuccessStatusCode();

        var tokens =
            await response.Content.ReadFromJsonAsync<TokensResponse>()
            ?? throw new InvalidOperationException("Токены не были получены.");

        foreach (var token in tokens.AuthTokens)
        {
            yield return $"TokenId: {token.TokenId}, Created: {token.Created}, LastUsed: {token.LastUsed}, ClientName: {token.ClientName}";
        }
    }
}
