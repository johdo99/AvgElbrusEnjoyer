using Client.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Client.Services;

public class ApiClient
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private readonly string _baseAddress = "http://localhost:8888";

    public ApiClient()
    {
    }

    /// <summary>
    /// Выполняет вход в систему
    /// </summary>
    public async Task<UserDto?> LoginAsync(string username, string password)
    {
        var requestData = new { Username = username, Password = password };
        var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseAddress}/api/auth/login", content);

        if (response.IsSuccessStatusCode)
        {
            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            return user;
        }

        return null;
    }

    /// <summary>
    /// Получает список всех компонентов
    /// </summary>
    public async Task<IEnumerable<ComponentDto>?> GetComponentsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseAddress}/api/catalog/components");
            if (response.IsSuccessStatusCode)
            {
                var components = await response.Content.ReadFromJsonAsync<IEnumerable<ComponentDto>>();
                return components;
            }
        }
        catch (HttpRequestException ex)
        {
            System.Console.WriteLine($"Ошибка подключения к серверу: {ex.Message}");
        }

        return null;
    }
}