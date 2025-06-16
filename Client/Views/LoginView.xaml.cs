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

        this.DataContext = viewModel;
    }
}