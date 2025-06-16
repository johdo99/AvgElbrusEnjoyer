using Client.Models;

namespace Client.ViewModels;

public class SelectableComponentViewModel : BaseViewModel
{
    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set { _isSelected = value; OnPropertyChanged(); }
    }

    public ComponentDto Component { get; }

    public string TypeAsString => Component.TypeAsString;

    public SelectableComponentViewModel(ComponentDto component)
    {
        Component = component;
    }
}