using Client.Models;
using Client.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Client.ViewModels;

public class CreateOrderViewModel : BaseViewModel
{
    private readonly ApiClient _apiClient;
    public ObservableCollection<ComponentDto> SelectedComponents { get; }
    public decimal TotalPrice { get; }

    public ICommand ConfirmOrderCommand { get; }

    public event Action<bool>? OnOrderProcessed;

    public CreateOrderViewModel(IEnumerable<ComponentDto> selectedComponents)
    {
        _apiClient = new ApiClient();
        SelectedComponents = new ObservableCollection<ComponentDto>(selectedComponents);
        TotalPrice = SelectedComponents.Sum(c => c.Price);
        ConfirmOrderCommand = new RelayCommand(async _ => await ConfirmOrderAsync());
    }

    private async Task ConfirmOrderAsync()
    {
        if (UserSession.CurrentUser == null)
        {
            OnOrderProcessed?.Invoke(false);
            return;
        }

        var orderRequest = new CreateOrderRequestDto
        {
            UserId = UserSession.CurrentUser.UserId,
            ComponentIds = SelectedComponents.Select(c => c.Id).ToList()
        };

        var success = await _apiClient.CreateOrderAsync(orderRequest);
        OnOrderProcessed?.Invoke(success);
    }
}