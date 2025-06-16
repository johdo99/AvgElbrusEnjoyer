using Client.Models;
using Client.Services;
using System.Collections.ObjectModel;

namespace Client.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly ApiClient _apiClient;

    public ObservableCollection<ComponentDto> Components { get; } = new();

    public MainViewModel()
    {
        _apiClient = new ApiClient();
        _ = LoadComponentsAsync();
    }

    private async Task LoadComponentsAsync()
    {
        var components = await _apiClient.GetComponentsAsync();
        if (components != null)
        {
            Components.Clear();
            foreach (var component in components)
            {
                Components.Add(component);
            }
        }
    }
}