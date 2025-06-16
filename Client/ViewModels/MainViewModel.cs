using Client.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Client.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly ApiClient _apiClient;

    public ObservableCollection<SelectableComponentViewModel> Components { get; } = new();

    public ICommand CreateBuildCommand { get; }

    public MainViewModel()
    {
        _apiClient = new ApiClient();
        CreateBuildCommand = new RelayCommand(_ => CreateBuild());
        _ = LoadComponentsAsync();
    }

    private void CreateBuild()
    {
        var selectedComponents = Components
            .Where(vm => vm.IsSelected)
            .Select(vm => vm.Component)
            .ToList();

        if (!selectedComponents.Any())
        {
            Debug.WriteLine("Не выбрано ни одного компонента.");
            return;
        }

        Debug.WriteLine("Выбранные компоненты для сборки:");
        decimal totalPrice = 0;
        foreach (var component in selectedComponents)
        {
            Debug.WriteLine($"- {component.Name} ({component.Price:C})");
            totalPrice += component.Price;
        }
        Debug.WriteLine($"Итоговая стоимость: {totalPrice:C}");
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
}