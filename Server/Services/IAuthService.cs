using Server.Models;
using System.Threading.Tasks;

namespace Server.Services;

public interface IAuthService
{
    /// <summary>
    /// Регистрирует нового пользователя
    /// </summary>
    /// <param name="username">Имя пользователя</param>
    /// <param name="password">Пароль в чистом виде</param>
    /// <returns>Созданный объект User</returns>
    Task<User> RegisterAsync(string username, string password);

    /// <summary>
    /// Выполняет вход пользователя в систему
    /// </summary>
    /// <param name="username">Имя пользователя</param>
    /// <param name="password">Пароль в чистом виде</param>
    /// <returns>Объект User в случае успеха, иначе null</returns>
    Task<User?> LoginAsync(string username, string password);
}