using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.Order;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DirectorViewModel : ViewModelBase

{
    private InventoryRepository _inventoryRepository;
    private OrderRepository _orderRepository;
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

    public DirectorViewModel(InventoryRepository inventoryRepository, OrderRepository orderRepository)
    {
        _inventoryRepository = inventoryRepository;
        _orderRepository = orderRepository;
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
        CurrentView = new DEquipmentPaneViewModel(_inventoryRepository, _orderRepository);
    }
}