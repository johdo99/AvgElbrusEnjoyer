using Client.Services;
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

    private readonly ApiClient _apiClient;
    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }

    public event Action? OnLoginSuccess;
    public event Action<string>? OnActionSuccess;
    public event Action<string>? OnActionFailed;

    public LoginViewModel()
    {
        _apiClient = new ApiClient();
        LoginCommand = new RelayCommand(async _ => await LoginAsync(), _ => CanExecute());
        RegisterCommand = new RelayCommand(async _ => await RegisterAsync(), _ => CanExecute());
    }

    private async Task LoginAsync()
    {
        var user = await _apiClient.LoginAsync(Username, Password);
        if (user != null)
        {
            OnLoginSuccess?.Invoke();
        }
        else
        {
            OnActionFailed?.Invoke("Неверный логин или пароль.");
        }
    }

    private async Task RegisterAsync()
    {
        var success = await _apiClient.RegisterAsync(Username, Password);
        if (success)
        {
            OnActionSuccess?.Invoke("Регистрация успешна! Теперь вы можете войти.");
        }
        else
        {
            OnActionFailed?.Invoke("Пользователь с таким именем уже существует.");
        }
    }

    private bool CanExecute()
    {
        return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
    }
}