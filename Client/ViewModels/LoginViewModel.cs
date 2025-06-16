using Client.Services;
using System.Diagnostics;
using System.Windows.Input;

namespace Client.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private string _username = string.Empty;
    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(); }
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); }
    }

    private readonly ApiClient _apiClient;
    public ICommand LoginCommand { get; }

    public LoginViewModel()
    {
        _apiClient = new ApiClient();
        LoginCommand = new RelayCommand(async _ => await LoginAsync(), _ => CanLogin());
    }

    public event Action? OnLoginSuccess;

    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;

        Debug.WriteLine($"[Client.LoginViewModel] Пытаемся войти. Пароль: '{Password}'");

        var user = await _apiClient.LoginAsync(Username, Password);
        if (user != null)
        {
            OnLoginSuccess?.Invoke();
        }
        else
        {
            ErrorMessage = "Неверный логин или пароль.";
        }
    }

    private bool CanLogin()
    {
        return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
    }
}