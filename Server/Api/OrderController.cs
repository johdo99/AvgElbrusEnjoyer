using Server.Services;
using System.Net;
using System.Text.Json;

namespace Server.Api;

public class OrderController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task HandleRequestAsync(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        if (request.HttpMethod == "POST")
        {
            using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
            var requestBody = await reader.ReadToEndAsync();
            var orderRequest = JsonSerializer.Deserialize<CreateOrderRequest>(requestBody);

            try
            {
                var newOrderId = await _orderService.CreateOrderFromComponentsAsync(orderRequest.UserId, orderRequest.ComponentIds);
                response.StatusCode = (int)HttpStatusCode.Created;
                var jsonResponse = JsonSerializer.Serialize(new { OrderId = newOrderId });
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonResponse);
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
        }
        response.OutputStream.Close();
    }
}