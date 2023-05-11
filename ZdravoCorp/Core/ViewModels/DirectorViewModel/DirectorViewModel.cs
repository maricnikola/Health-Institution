using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.Order;
using ZdravoCorp.Core.Repositories.Room;
using ZdravoCorp.Core.Repositories.Transfers;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DirectorViewModel : ViewModelBase

{
    private InventoryRepository _inventoryRepository;
    private RoomRepository _roomRepository;
    private OrderRepository _orderRepository;
    private TransferRepository _transferRepository;
    private object _currentView;

    public ICommand ViewEquipmentCommand { get; private set; }
    public ICommand ViewDynamicEquipmentCommand { get; private set; }
    public ICommand MoveDynamicEquipmentCommand { get; private set; }
    public ICommand MoveEquipmentCommand { get; private set; }


    public object CurrentView
    {
        get { return _currentView; }
        set
        {
            _currentView = value;
            OnPropertyChanged("CurrentView");
        }
    }

    public DirectorViewModel(InventoryRepository inventoryRepository, OrderRepository orderRepository, RoomRepository roomRepository, TransferRepository transferRepository)
    {
        _inventoryRepository = inventoryRepository;
        _orderRepository = orderRepository;
        _roomRepository = roomRepository;
        _transferRepository = transferRepository;
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
        CurrentView = new MoveDEquipmentViewModel(_inventoryRepository, _roomRepository);
    }

    public void MoveEquipmentView()
    {
        CurrentView = new MoveEquipmentViewModel(_inventoryRepository, _roomRepository, _transferRepository);
    }
}