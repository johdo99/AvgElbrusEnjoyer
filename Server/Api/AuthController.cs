using Server.Services;
using System.Net;
using System.Text.Json;

namespace Server.Api;

public class AuthController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task HandleRequestAsync(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        try
        {
            if (request.Url.AbsolutePath.EndsWith("/register") && request.HttpMethod == "POST")
            {
                await RegisterUserAsync(request, response);
            }
            else if (request.Url.AbsolutePath.EndsWith("/login") && request.HttpMethod == "POST")
            {
                await LoginUserAsync(request, response);
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
        catch (Exception ex)
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Console.WriteLine(ex.Message);
        }
        finally
        {
            response.OutputStream.Close();
        }
    }

    private async Task RegisterUserAsync(HttpListenerRequest request, HttpListenerResponse response)
    {
        using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
        var requestBody = await reader.ReadToEndAsync();
        var registerRequest = JsonSerializer.Deserialize<RegisterRequest>(requestBody);

        if (registerRequest == null)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        try
        {
            var newUser = await _authService.RegisterAsync(registerRequest.Username, registerRequest.Password);
            response.StatusCode = (int)HttpStatusCode.Created;
        }
        catch (InvalidOperationException ex)
        {
            response.StatusCode = (int)HttpStatusCode.Conflict; // 409 - конфликт, пользователь уже есть
            Console.WriteLine(ex.Message);
        }
    }

    private async Task LoginUserAsync(HttpListenerRequest request, HttpListenerResponse response)
    {
        using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
        var requestBody = await reader.ReadToEndAsync();
        var loginRequest = JsonSerializer.Deserialize<LoginRequest>(requestBody);

        if (loginRequest == null)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        var user = await _authService.LoginAsync(loginRequest.Username, loginRequest.Password);

        if (user == null)
        {
            response.StatusCode = (int)HttpStatusCode.Unauthorized; // 401 - не авторизован
            return;
        }

        response.StatusCode = (int)HttpStatusCode.OK;
        // В реальном приложении здесь бы возвращался токен (например, JWT)
        var responseData = new { UserId = user.Id, Username = user.Username, Role = user.Role.ToString() };
        var jsonResponse = JsonSerializer.Serialize(responseData);
        var buffer = System.Text.Encoding.UTF8.GetBytes(jsonResponse);

        response.ContentType = "application/json";
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
    }
}