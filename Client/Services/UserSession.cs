using Client.Models;

namespace Client.Services;

public static class UserSession
{
    public static UserDto? CurrentUser { get; private set; }

    public static void Login(UserDto user)
    {
        CurrentUser = user;
    }

    public static void Logout()
    {
        CurrentUser = null;
    }
}