using Client.Models;
using Client.ViewModels;
using System.Windows;

namespace Client.Views;

public partial class CreateOrderView : Window
{
    public CreateOrderView(IEnumerable<ComponentDto> selectedComponents)
    {
        InitializeComponent();
        DataContext = new CreateOrderViewModel(selectedComponents);
    }
}