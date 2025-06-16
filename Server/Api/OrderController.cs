using Server.Api.DTOs;
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

        try
        {
            if (request.HttpMethod == "GET")
            {
                await GetUserOrdersAsync(request, response);
            }
            else if (request.HttpMethod == "POST")
            {
                await CreateOrderAsync(request, response);
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
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
    private async Task GetUserOrdersAsync(HttpListenerRequest request, HttpListenerResponse response)
    {
        var userIdStr = request.QueryString["userId"];
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        var orders = await _orderService.GetOrdersForUserAsync(userId);

        var orderDtos = orders.Select(o => new OrderDto
        {
            OrderId = o.Id,
            OrderDate = o.OrderDate,
            Status = o.Status,
            TotalPrice = o.TotalPrice
        }).ToList();

        response.StatusCode = (int)HttpStatusCode.OK;
        response.ContentType = "application/json";

        var jsonResponse = JsonSerializer.Serialize(orderDtos);
        var buffer = System.Text.Encoding.UTF8.GetBytes(jsonResponse);
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
    }

    private async Task CreateOrderAsync(HttpListenerRequest request, HttpListenerResponse response)
    {
        using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
        var requestBody = await reader.ReadToEndAsync();
        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var orderRequest = JsonSerializer.Deserialize<CreateOrderRequest>(requestBody, jsonOptions);

        if (orderRequest?.ComponentIds == null)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        var newOrderId = await _orderService.CreateOrderFromComponentsAsync(orderRequest.UserId, orderRequest.ComponentIds);
        response.StatusCode = (int)HttpStatusCode.Created;
        var jsonResponse = JsonSerializer.Serialize(new { OrderId = newOrderId });
        var buffer = System.Text.Encoding.UTF8.GetBytes(jsonResponse);
        response.ContentType = "application/json";
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
    }
}