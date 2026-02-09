using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace ZontApi;

public class ZontRepository(ZontCredentials credentials, HttpClient httpClient)
{
    public string token = string.Empty;

    public async Task AuthorizeAsync()
    {
        var baseUrl = "https://my.zont.online/api/widget/v3/";

        // 1. Формирование заголовка Basic Auth
        // Кодируем строку "логин:пароль" в Base64
        var authString = $"{credentials.Login}:{credentials.Password}";
        var base64AuthString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authString));

        // Добавляем заголовок Authorization
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
            "Basic",
            base64AuthString
        );
        httpClient.DefaultRequestHeaders.Add("X-ZONT-Client", credentials.Email);

        // 2. Отправка POST запроса на эндпоинт получения токена
        // Документация не требует тела запроса для этого POST-метода.
        var requestBody = new { client_name = "Zont" };
        var response = await httpClient.PostAsJsonAsync($"{baseUrl}authtokens", requestBody);

        response.EnsureSuccessStatusCode(); // Выбросит исключение при ошибке 400-500

        // 3. Чтение токена из ответа
        // API, скорее всего, вернет объект JSON с полем "token"
        var authResponse =
            await response.Content.ReadFromJsonAsync<ZontAuthResponse>()
            ?? throw new InvalidOperationException("Токен не был получен.");

        token = authResponse.Token;
    }

    public async IAsyncEnumerable<string> GetTokensAsync()
    {
        yield return token;
    }
}

public record ZontCredentials(
    [Required] string Login,
    [Required] string Password,
    [Required] string Email);

internal record ZontAuthResponse(
    bool Ok,
    string Token,
    string TokenId);