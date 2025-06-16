using Client.ViewModels;
using System.Windows;

namespace Client.Views;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
        var viewModel = new LoginViewModel();

        viewModel.OnLoginSuccess += () =>
        {
            var mainView = new MainView();
            mainView.Show();
            this.Close();
        };

        viewModel.OnActionSuccess += (message) =>
        {
            MessageBox.Show(message, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        };

        viewModel.OnActionFailed += (errorMessage) =>
        {
            MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        };

        this.DataContext = viewModel;
    }
}