using Client.ViewModels;
using System.Windows;

namespace Client.Views;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();

        if (DataContext is MainViewModel viewModel)
        {
            viewModel.OnCreateBuildRequested += (selectedComponents) =>
            {
                var createOrderView = new CreateOrderView(selectedComponents)
                {
                    Owner = this
                };

                if (createOrderView.ShowDialog() == true)
                {
                    _ = viewModel.LoadOrdersAsync();
                }
            };
        }
    }
}