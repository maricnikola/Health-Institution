using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Orders;
using ZdravoCorp.Core.Repositories.Equipment;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.Order;
using ZdravoCorp.View.DirectorView;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DEquipmentPaneViewModel : ViewModelBase
{
    private InventoryRepository _inventoryRepository;
    private EquipmentRepository _equipmentRepository;
    private OrderRepository _orderRepository;
    private ObservableCollection<DynamicInventoryViewModel> _dynamicInventory;
    private object _lock;
    private object _lock2;
    private ObservableCollection<OrderViewModel> _orders;

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

    public DEquipmentPaneViewModel(InventoryRepository inventoryRepository, OrderRepository orderRepository, EquipmentRepository equipmentRepository)
    {
        _lock = new object();
        _lock2 = new object();
        _dynamicInventory = new ObservableCollection<DynamicInventoryViewModel>();
        _inventoryRepository = inventoryRepository;
        _equipmentRepository = equipmentRepository;
        _orderRepository = orderRepository;
        _orders = new ObservableCollection<OrderViewModel>();
        
        BindingOperations.EnableCollectionSynchronization(_dynamicInventory, _lock);
        BindingOperations.EnableCollectionSynchronization(_orders, _lock2);
        
        _inventoryRepository.OnRequestUpdate += (s, e) => RefreshInventory();
        _orderRepository.OnRequestUpdate += (s, e) => RefreshOrders();
        foreach (var inventoryItem in _inventoryRepository.GetDynamicGrouped())
        {
            if (inventoryItem.Quantity < 5)
                _dynamicInventory.Add(new DynamicInventoryViewModel(inventoryItem));
        }

        CreateOrder = new DelegateCommand(o => OrderConfirmDialog());
        RefreshOrders();
    }

    private void RefreshInventory()
    {
        lock (_lock)
        {
            var updateInventory = new ObservableCollection<DynamicInventoryViewModel>();
            foreach (var inventoryItem in _inventoryRepository.GetDynamicGrouped())
            {
                if (inventoryItem.Quantity < 5)
                    updateInventory.Add(new DynamicInventoryViewModel(inventoryItem));
            }

            DynamicInventory = updateInventory;
        }
    }

    private void RefreshOrders()
    {
        lock (_lock2)
        {
            var updateOrders = new ObservableCollection<OrderViewModel>();
            string items;
            foreach (var order in _orderRepository.GetOrders())
            {
                updateOrders.Add(new OrderViewModel(order, ParseItemsDictionary(order.Items)));
            }

            Orders = updateOrders;
        }
        
    }

    private string ParseItemsDictionary(Dictionary<int, int> items)
    {
        string parsedItems = "";
        foreach (var (key, value) in items)
        {
            parsedItems += _equipmentRepository.GetById(key).Name + " : " + value.ToString() +"   ";
        }

        return parsedItems;


    }

    private void OrderConfirmDialog()
    {
        var vm = new DEquipmentOrderConfirmViewModel(DynamicInventory.Where(item => item.IsChecked), _orderRepository,
            _inventoryRepository);
        var confirmDialog = new DynamicOrderConfirmView() { DataContext = vm };
        vm.OnRequestClose += (s, e) => confirmDialog.Close();
        confirmDialog.Show();
    }
}