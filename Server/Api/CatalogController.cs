using Server.Services;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Api;

public class CatalogController
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    public async Task HandleRequestAsync(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        if (request.HttpMethod != "GET")
        {
            response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            response.OutputStream.Close();
            return;
        }

        var components = await _catalogService.GetAllComponentsAsync();

        response.StatusCode = (int)HttpStatusCode.OK;
        response.ContentType = "application/json";

        var jsonResponse = JsonSerializer.Serialize(components);
        var buffer = System.Text.Encoding.UTF8.GetBytes(jsonResponse);

        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }
}