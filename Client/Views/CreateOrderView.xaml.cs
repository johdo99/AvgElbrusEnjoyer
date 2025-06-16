using Client.Models;
using Client.ViewModels;
using System.Collections.Generic;
using System.Windows;

namespace Client.Views;

public partial class CreateOrderView : Window
{
    public CreateOrderView(IEnumerable<ComponentDto> selectedComponents)
    {
        InitializeComponent();
        var viewModel = new CreateOrderViewModel(selectedComponents);

        viewModel.OnOrderProcessed += (success) =>
        {
            if (success)
            {
                MessageBox.Show("Заказ успешно создан!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Произошла ошибка при создании заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
        };

        DataContext = viewModel;
    }
}