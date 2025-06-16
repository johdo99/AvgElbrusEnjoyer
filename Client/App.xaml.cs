using Client.Views;
using System.Windows;

namespace Client;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var loginView = new LoginView();
        loginView.Show();
    }
}