using System.Diagnostics;
using Server.Data.Repositories;
using Server.Models;

namespace Server.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> RegisterAsync(string username, string password)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(username);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Пользователь с таким именем уже существует.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        var newUser = new User
        {
            Username = username,
            PasswordHash = passwordHash,
            Role = UserRole.Client // По умолчанию все новые пользователи - клиенты
        };

        await _userRepository.AddAsync(newUser);

        return newUser;
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        Debug.WriteLine($"[Server.AuthService] Дошло до сервиса. Пароль: '{password}'");
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            return null;
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return null;
        }

        return user;
    }
}