using CottageSensorAggregator.ZontApi.Auth;
using CottageSensorAggregator.ZontApi.Device;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

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

    public async Task<string> GetDevices()
    {
        var response = await s_httpClient.GetAsync($"{_appSettings.ZontSettings.ApiUrl}devices");
        response.EnsureSuccessStatusCode();

        string jsonString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(jsonString);

        var jObject = JsonNode.Parse(jsonString);

        if (jObject == null || !jObject["ok"]!.GetValue<bool>())
        {
            var error = $"""error: {jObject["error"]}{Environment.NewLine}error_ui: {jObject["error_ui"]}""";
            throw new InvalidOperationException(error);
        }

        var jsonDeserializeOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };
        var deviceResponse =
            JsonSerializer.Deserialize<DeviceResponse>(jsonString, jsonDeserializeOptions)
            ?? throw new InvalidOperationException($"Не удалось распарсить json:{Environment.NewLine}{jsonString}");

        var jsonSerialiseOptions = new JsonSerializerOptions
        {
            WriteIndented = true,  // форматирование с отступами
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        return JsonSerializer.Serialize(deviceResponse, jsonSerialiseOptions);
    }
}
