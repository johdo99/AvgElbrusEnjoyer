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
        IAuthService authService = new AuthService(userRepository);
        var authController = new AuthController(authService);

        IComponentRepository componentRepository = new ComponentRepository(dbConnectionFactory);
        ICatalogService catalogService = new CatalogService(componentRepository);
        var catalogController = new CatalogController(catalogService);

        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8888/");
        listener.Start();
        Console.WriteLine("Сервер запущен на http://localhost:8888/");

        while (true)
        {
            var context = await listener.GetContextAsync();
            var request = context.Request;
            Console.WriteLine($"Получен запрос: {request.HttpMethod} {request.Url}");

            if (request.Url.AbsolutePath.StartsWith("/api/auth"))
            {
                await authController.HandleRequestAsync(context);
            }
            else if (request.Url.AbsolutePath.StartsWith("/api/catalog"))
            {
                await catalogController.HandleRequestAsync(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.OutputStream.Close();
            }
        }
    }
}