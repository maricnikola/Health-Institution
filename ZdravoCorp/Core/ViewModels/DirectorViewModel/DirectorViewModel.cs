using System.Windows.Input;
using Autofac;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.EquipmentRepo;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.OrderRepo;
using ZdravoCorp.Core.Repositories.RoomRepo;
using ZdravoCorp.Core.Repositories.TransfersRepo;
using ZdravoCorp.Core.Services.EquipmentServices;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.OrderServices;
using ZdravoCorp.Core.Services.RoomServices;
using ZdravoCorp.Core.Services.TransferServices;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DirectorViewModel : ViewModelBase

{
    private object _currentView;
    private readonly IEquipmentService _equipmentService;
    private readonly IInventoryService _inventoryService;
    private readonly IOrderService _orderService;
    private readonly IRoomService _roomService;
    private readonly ITransferService _transferService;

    public DirectorViewModel()
    {
        _equipmentService = Injector.Container.Resolve<IEquipmentService>();
        _inventoryService = Injector.Container.Resolve<IInventoryService>();
        ViewEquipmentCommand = new DelegateCommand(o => EquipmentView());
        MoveEquipmentCommand = new DelegateCommand(o => MoveEquipmentView());
        ViewDynamicEquipmentCommand = new DelegateCommand(o => DynamicEquipmentView());
        MoveDynamicEquipmentCommand = new DelegateCommand(o => MoveDynamicEquipmentView());
        _currentView = new EquipmentPaneViewModel(_inventoryService);
    }

    public ICommand ViewEquipmentCommand { get; private set; }
    public ICommand ViewDynamicEquipmentCommand { get; private set; }
    public ICommand MoveDynamicEquipmentCommand { get; private set; }
    public ICommand MoveEquipmentCommand { get; private set; }


    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public void EquipmentView()
    {
        CurrentView = new EquipmentPaneViewModel(_inventoryService);
    }

    public void DynamicEquipmentView()
    {
        CurrentView = new DEquipmentPaneViewModel(_inventoryService, _equipmentService, _orderService);
    }

    public void MoveDynamicEquipmentView()
    {
        CurrentView = new MoveDEquipmentViewModel(_inventoryService, _roomService);
    }

    public void MoveEquipmentView()
    {
        CurrentView = new MoveEquipmentViewModel(_inventoryService, _roomService, _transferService);
    }
}