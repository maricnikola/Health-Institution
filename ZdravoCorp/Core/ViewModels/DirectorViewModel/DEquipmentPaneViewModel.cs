using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories.EquipmentRepo;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.OrderRepo;
using ZdravoCorp.Core.Services.EquipmentServices;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.OrderServices;
using ZdravoCorp.View.DirectorView;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DEquipmentPaneViewModel : ViewModelBase
{
    private ObservableCollection<DynamicInventoryViewModel> _dynamicInventory;
    private readonly IEquipmentService _equipmentService;
    private readonly IInventoryService _inventoryService;
    private readonly IOrderService _orderService;
    private readonly object _lock;
    private readonly object _lock2;
    
    private ObservableCollection<OrderViewModel> _orders;

    public DEquipmentPaneViewModel(IInventoryService inventoryService, IEquipmentService equipmentService, IOrderService orderService)
    {
        _lock = new object();
        _lock2 = new object();
        _dynamicInventory = new ObservableCollection<DynamicInventoryViewModel>();
        _equipmentService = equipmentService;
        _inventoryService = inventoryService;
        _orderService = orderService;
        _orders = new ObservableCollection<OrderViewModel>();

        BindingOperations.EnableCollectionSynchronization(_dynamicInventory, _lock);
        BindingOperations.EnableCollectionSynchronization(_orders, _lock2);

        _inventoryService.DataChanged += (s, e) => RefreshInventory();
        _orderService.DataChanged += (s, e) => RefreshOrders();
        foreach (var inventoryItem in _inventoryService.GetDynamicGrouped())
            if (inventoryItem.Quantity < 5)
                _dynamicInventory.Add(new DynamicInventoryViewModel(inventoryItem));

        CreateOrder = new DelegateCommand(o => OrderConfirmDialog());
        RefreshOrders();
    }

    public IEnumerable<OrderViewModel> Orders
    {
        get => _orders;
        set
        {
            _orders = new ObservableCollection<OrderViewModel>(value);
            OnPropertyChanged();
        }
    }

    public ICommand CreateOrder { get; }

    public IEnumerable<DynamicInventoryViewModel> DynamicInventory

    {
        get => _dynamicInventory;
        set
        {
            _dynamicInventory = new ObservableCollection<DynamicInventoryViewModel>(value);
            OnPropertyChanged();
        }
    }

    private void RefreshInventory()
    {
        lock (_lock)
        {
            var updateInventory = new ObservableCollection<DynamicInventoryViewModel>();
            foreach (var inventoryItem in _inventoryService.GetDynamicGrouped())
                if (inventoryItem.Quantity < 5)
                    updateInventory.Add(new DynamicInventoryViewModel(inventoryItem));

            DynamicInventory = updateInventory;
        }
    }

    private void RefreshOrders()
    {
        lock (_lock2)
        {
            var updateOrders = new ObservableCollection<OrderViewModel>();
            string items;
            foreach (var order in _orderService.GetAll())
                updateOrders.Add(new OrderViewModel(order, ParseItemsDictionary(order.Items)));

            Orders = updateOrders;
        }
    }

    private string ParseItemsDictionary(Dictionary<int, int> items)
    {
        var parsedItems = "";
        foreach (var (key, value) in items)
            parsedItems += _equipmentService.GetById(key).Name + " : " + value + "   ";

        return parsedItems;
    }

    private void OrderConfirmDialog()
    {
        var vm = new DEquipmentOrderConfirmViewModel(DynamicInventory.Where(item => item.IsChecked), _orderService);
        var confirmDialog = new DynamicOrderConfirmView { DataContext = vm };
        vm.OnRequestClose += (s, e) => confirmDialog.Close();
        confirmDialog.Show();
    }
}