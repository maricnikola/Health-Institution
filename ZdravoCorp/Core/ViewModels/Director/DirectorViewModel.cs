using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories.Inventory;

namespace ZdravoCorp.Core.ViewModels;

public class DirectorViewModel : ViewModelBase

{
    private InventoryRepository _inventoryRepository;
    private object _currentView;

    public ICommand LoadEquipmentCommand { get; private set; }
    public ICommand LoadDynamicEquipmentCommand { get; private set; }
    
    
    public object CurrentView
    {
        get
        {
            return _currentView;
        }
        set
        {
            _currentView = value;
            OnPropertyChanged("CurrentView");
        }
    }

    public DirectorViewModel(InventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
        LoadEquipmentCommand = new DelegateCommand(o => LoadEquipment());
        LoadDynamicEquipmentCommand = new DelegateCommand(o => LoadDynamicEquipment());
        _currentView = new EquipmentPaneViewModel(_inventoryRepository);
    }

    public void LoadEquipment()
    {
        CurrentView = new EquipmentPaneViewModel(_inventoryRepository);
    }

    public void LoadDynamicEquipment()
    {
        CurrentView = new DynamicEquipmentViewModel();
    }
}