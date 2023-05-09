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

    public ICommand ViewEquipmentCommand { get; private set; }
    public ICommand ViewDynamicEquipmentCommand { get; private set; }
    public ICommand MoveDynamicEquipmentCommand { get; private set; }
    public ICommand MoveEquipmentCommand { get; private set; }
    
    
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
       ViewEquipmentCommand = new DelegateCommand(o => EquipmentView());
       MoveEquipmentCommand = new DelegateCommand(o => MoveEquipmentView());
        ViewDynamicEquipmentCommand = new DelegateCommand(o => DynamicEquipmentView());
        MoveDynamicEquipmentCommand = new DelegateCommand(o => MoveDynamicEquipmentView());
        _currentView = new EquipmentPaneViewModel(_inventoryRepository);
    }

    public void EquipmentView()
    {
        CurrentView = new EquipmentPaneViewModel(_inventoryRepository);
    }

    public void DynamicEquipmentView()
    {
        CurrentView = new DEquipmentPaneViewModel(_inventoryRepository, _orderRepository);
    }
    
    public void MoveDynamicEquipmentView()
    {
        CurrentView = new MoveDEquipmentViewModel();
    }
    
    public void MoveEquipmentView()
    {
        CurrentView = new MoveEquipmentViewModel();
    }
}