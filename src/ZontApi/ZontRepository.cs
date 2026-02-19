using CottageSensorAggregator.ZontApi.Auth;
using CottageSensorAggregator.ZontApi.Device;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
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
    private readonly ILogger<ZontRepository> _logger;

    public ZontRepository(
        IOptionsSnapshot<AppSettings> appSettings,
        IHttpClientFactory httpClientFactory,
        ILogger<ZontRepository> logger)
    {
        s_httpClient = httpClientFactory.CreateClient(AppSettings.ZontHttpClientName);
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public async Task AuthorizeAsync(CancellationToken cancellationToken)
    {
        var requestBody = new { client_name = AppSettings.AppName };

        var response = await s_httpClient.PostAsJsonAsync($"{_appSettings.ZontSettings.ApiUrl}authtokens", requestBody, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError(
                "Ошибка.\nСтатус: {StatusCode}\nОтвет: {ErrorContent}",
                response.StatusCode, errorContent);
            throw new HttpRequestException($"Ошибка авторизации: {response.StatusCode}");
        }

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken);

        if (authResponse == null || string.IsNullOrEmpty(authResponse.Token))
        {
            _logger.LogError("Ответ получен, но токен отсутствует или не десериализован");
            _logger.LogError($"Ответ:{Environment.NewLine}{response.Content.ToString()}");
            throw new InvalidOperationException("Токен не был получен.");
        }

        var token = authResponse.Token;

        s_httpClient.DefaultRequestHeaders.Add("X-ZONT-Token", $"{token}");
    }

    public async IAsyncEnumerable<string> GetTokensAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var response = await s_httpClient.GetAsync($"{_appSettings.ZontSettings.ApiUrl}authtokens", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError(
                "Ошибка.\nСтатус: {StatusCode}\nОтвет: {ErrorContent}",
                response.StatusCode, errorContent);
            throw new HttpRequestException($"Ошибка получения токенов: {response.StatusCode}");
        }

        var tokens = await response.Content.ReadFromJsonAsync<TokensResponse>(cancellationToken);

        if (tokens == null)
        {
            _logger.LogError("Ответ получен, но не десериализован");
            _logger.LogError($"Ответ:{Environment.NewLine}{response.Content.ToString()}");
            throw new InvalidOperationException("Не удалось дессериализовать токены.");
        }

        foreach (var token in tokens.AuthTokens)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return $"TokenId: {token.TokenId}, Created: {token.Created}, LastUsed: {token.LastUsed}, ClientName: {token.ClientName}";
        }
    }

    public async Task<string> GetDevices(CancellationToken cancellationToken)
    {
        var response = await s_httpClient.GetAsync($"{_appSettings.ZontSettings.ApiUrl}devices", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError(
                "Ошибка.\nСтатус: {StatusCode}\nОтвет: {ErrorContent}",
                response.StatusCode, errorContent);
            throw new HttpRequestException($"Ошибка: {response.StatusCode}");
        }

        string jsonString = await response.Content.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(jsonString))
        {
            _logger.LogError("Ответ пустой.");
            _logger.LogError($"Ответ:{Environment.NewLine}{response.Content.ToString()}");
            throw new InvalidOperationException("Пришел пустой ответ.");
        }

        var jObject = JsonNode.Parse(jsonString);

        if (jObject == null || !jObject["ok"]!.GetValue<bool>())
        {
            var error = $"""error: {jObject?["error"]}{Environment.NewLine}error_ui: {jObject?["error_ui"]}""";
            _logger.LogError(error);
            throw new InvalidOperationException(error);
        }

        var jsonDeserializeOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };

        DeviceResponse? deviceResponse = null;

        try
        {
            deviceResponse = JsonSerializer.Deserialize<DeviceResponse>(jsonString, jsonDeserializeOptions);

            if (deviceResponse == null)
            {
                _logger.LogError("Не удалось дессериализовать ответ.");
                _logger.LogError($"Ответ:{Environment.NewLine}{jsonString}");
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, ex.Message, ex.StackTrace);
            throw;
        }
        catch (NotSupportedException ex)
        {
            _logger.LogError(ex, ex.Message, ex.StackTrace);
        }

        var jsonSerialiseOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(deviceResponse, jsonSerialiseOptions);
    }
}
