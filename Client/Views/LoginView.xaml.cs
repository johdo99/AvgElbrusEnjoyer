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
            this.Close();
        };

        this.DataContext = viewModel;
    }
}