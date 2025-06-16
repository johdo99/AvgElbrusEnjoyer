using Client.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Client.ViewModels;

public class CreateOrderViewModel : BaseViewModel
{
    public ObservableCollection<ComponentDto> SelectedComponents { get; }

    public decimal TotalPrice { get; }

    public ICommand ConfirmOrderCommand { get; }

    public CreateOrderViewModel(IEnumerable<ComponentDto> selectedComponents)
    {
        SelectedComponents = new ObservableCollection<ComponentDto>(selectedComponents);
        TotalPrice = SelectedComponents.Sum(c => c.Price);
        ConfirmOrderCommand = new RelayCommand(_ => ConfirmOrder());
    }

    private void ConfirmOrder()
    {
    }
}