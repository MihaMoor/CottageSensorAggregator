using CottageSensorAggregator.Core.Loggers;
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
    private readonly ZontSettings _zontSettings;
    private readonly ILogger<ZontRepository> _logger;

    public ZontRepository(
        IOptions<ZontSettings> zontSettings,
        IHttpClientFactory httpClientFactory,
        ApplicationLogger<ZontRepository> logger)
    {
        _zontSettings = zontSettings.Value;
        s_httpClient = httpClientFactory.CreateClient(AppSettings.ZontHttpClientName);
        _logger = logger;
    }

    public async Task<bool> AuthorizeAsync(CancellationToken cancellationToken)
    {
        var requestBody = new { client_name = AppSettings.AppName };

        var response = await s_httpClient.PostAsJsonAsync($"{_zontSettings.ApiUrl}authtokens", requestBody, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError(
                "Ошибка.\nСтатус: {StatusCode}\nОтвет: {ErrorContent}",
                response.StatusCode, errorContent);

            return false;
        }

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken);

        if (authResponse == null || string.IsNullOrEmpty(authResponse.Token))
        {
            _logger.LogError("Ответ получен, но токен отсутствует или не десериализован");
            _logger.LogError($"Ответ:{Environment.NewLine}{response.Content.ToString()}");

            return false;
        }

        var token = authResponse.Token;

        s_httpClient.DefaultRequestHeaders.Add("X-ZONT-Token", $"{token}");
        _logger.LogInformation(
            "Токен успешно получен. AccessToken (первые 10 символов): {AccessTokenPrefix}",
            token.Substring(0, Math.Min(10, token.Length)));

        return true;
    }

    public async IAsyncEnumerable<string> GetTokensAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var tokens = await GetTokens(cancellationToken);

        foreach (var token in tokens.AuthTokens)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var str = $"TokenId: {token.TokenId}, Created: {token.Created}, LastUsed: {token.LastUsed}, ClientName: {token.ClientName}";
            _logger.LogInformation(str);

            yield return str;
        }
    }

    public async Task<string> GetDevicesAsync(CancellationToken cancellationToken)
    {
        var response = await s_httpClient.GetAsync($"{_zontSettings.ApiUrl}devices", cancellationToken);

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

        var serializedDeviceResponse = JsonSerializer.Serialize(deviceResponse, jsonSerialiseOptions);
        _logger.LogInformation(serializedDeviceResponse);

        return serializedDeviceResponse;
    }

    public async Task DeleteTokensAsync(string[] tokenIds, CancellationToken cancellationToken)
    {
        foreach (var token in tokenIds)
        {
            try
            {
                await s_httpClient.DeleteAsync($"{_zontSettings.ApiUrl}authtokens/{token}", cancellationToken);
                _logger.LogInformation($"Удален токен:\nToken id = {token}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Не удалось удалить токен: Token id = '{token}'");
                _logger.LogWarning(ex, ex.Message, ex.StackTrace);
            }
        }
    }

    private async Task<TokensResponse> GetTokens(CancellationToken cancellationToken)
    {
        var response = await s_httpClient.GetAsync($"{_zontSettings.ApiUrl}authtokens", cancellationToken);

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

        return tokens;
    }
}
