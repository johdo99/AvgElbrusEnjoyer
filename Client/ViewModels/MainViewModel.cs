using Client.Models;
using Client.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Client.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly ApiClient _apiClient;

    public ObservableCollection<SelectableComponentViewModel> Components { get; } = new();
    public ObservableCollection<OrderDto> Orders { get; } = new();

    public ICommand CreateBuildCommand { get; }
    public event Action<IEnumerable<ComponentDto>>? OnCreateBuildRequested;

    public MainViewModel()
    {
        _apiClient = new ApiClient();
        CreateBuildCommand = new RelayCommand(_ => CreateBuild());

        _ = LoadComponentsAsync();
        _ = LoadOrdersAsync();
    }

    private void CreateBuild()
    {
        var selectedComponents = Components
            .Where(vm => vm.IsSelected)
            .Select(vm => vm.Component)
            .ToList();

        if (!selectedComponents.Any())
        {
            return;
        }

        OnCreateBuildRequested?.Invoke(selectedComponents);
    }

    private async Task LoadComponentsAsync()
    {
        var components = await _apiClient.GetComponentsAsync();
        if (components != null)
        {
            Components.Clear();
            foreach (var component in components)
            {
                Components.Add(new SelectableComponentViewModel(component));
            }
        }
    }

    private async Task LoadOrdersAsync()
    {
        var orders = await _apiClient.GetMyOrdersAsync();
        if (orders != null)
        {
            Orders.Clear();
            foreach (var order in orders)
            {
                Orders.Add(order);
            }
            Debug.WriteLine($"[MainViewModel] Успешно загружено {orders.Count()} заказов.");
        }
    }
}