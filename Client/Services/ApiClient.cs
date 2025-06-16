using Client.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;

namespace Client.Services;

public class ApiClient
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private readonly string _baseAddress = "http://localhost:8888";

    public async Task<UserDto?> LoginAsync(string username, string password)
    {
        var requestData = new LoginRequestDto { Username = username, Password = password };
        Debug.WriteLine($"[Client.ApiClient] Отправляем на сервер. Пароль в DTO: '{requestData.Password}'");
        var response = await _httpClient.PostAsJsonAsync($"{_baseAddress}/api/auth/login", requestData);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }
        return null;
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        var requestData = new RegisterRequestDto { Username = username, Password = password };
        var response = await _httpClient.PostAsJsonAsync($"{_baseAddress}/api/auth/register", requestData);

        return response.StatusCode == System.Net.HttpStatusCode.Created;
    }

    public async Task<IEnumerable<ComponentDto>?> GetComponentsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseAddress}/api/catalog/components");

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("[ApiClient] Успешно получили компоненты (Статус 200 OK).");
                return await response.Content.ReadFromJsonAsync<IEnumerable<ComponentDto>>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[ApiClient] Ошибка получения компонентов! Статус-код: {response.StatusCode}. Ответ сервера: {errorContent}");
                return null;
            }
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"[ApiClient] Критическая ошибка подключения к серверу: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> CreateOrderAsync(CreateOrderRequestDto orderRequest)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseAddress}/api/orders", orderRequest);
        return response.StatusCode == System.Net.HttpStatusCode.Created;
    }
}