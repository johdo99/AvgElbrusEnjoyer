using Server.Api;
using Server.Data;
using Server.Data.Repositories;
using Server.Services;
using System.Net;

namespace Server;

public class Program
{
    public static async Task Main(string[] args)
    {
        var dbConnectionFactory = new DbConnectionFactory();

        IUserRepository userRepository = new UserRepository(dbConnectionFactory);
        IComponentRepository componentRepository = new ComponentRepository(dbConnectionFactory);
        IOrderRepository orderRepository = new OrderRepository(dbConnectionFactory);

        IAuthService authService = new AuthService(userRepository);
        ICatalogService catalogService = new CatalogService(componentRepository);
        IOrderService orderService = new OrderService(orderRepository, componentRepository);

        var authController = new AuthController(authService);
        var catalogController = new CatalogController(catalogService);
        var orderController = new OrderController(orderService);

        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8888/");
        listener.Start();
        Console.WriteLine("Сервер запущен на http://localhost:8888/");

        while (true)
        {
            var context = await listener.GetContextAsync();
            var request = context.Request;
            Console.WriteLine($"Получен запрос: {request.HttpMethod} {request.Url}");

            var path = request.Url.AbsolutePath;
            if (path.StartsWith("/api/auth"))
            {
                await authController.HandleRequestAsync(context);
            }
            else if (path.StartsWith("/api/catalog"))
            {
                await catalogController.HandleRequestAsync(context);
            }
            else if (path.StartsWith("/api/orders"))
            {
                await orderController.HandleRequestAsync(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.OutputStream.Close();
            }
        }
    }
}