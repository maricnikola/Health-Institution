﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services.Services;
using ZdravoCorp.GUI.HospitalAssets.Equipment.Views;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalAssets.Equipment.ViewModels;

public class DynamicEquipmentPaneViewModel : ViewModelBase
{
    private ObservableCollection<DynamicInventoryViewModel> _dynamicInventory;
    private readonly IEquipmentService _equipmentService;
    private readonly IInventoryService _inventoryService;
    private readonly IOrderService _orderService;
    private readonly object _lock;
    private readonly object _lock2;
    
    private ObservableCollection<OrderViewModel> _orders;

    public DynamicEquipmentPaneViewModel(IInventoryService inventoryService, IEquipmentService equipmentService, IOrderService orderService)
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

        CreateOrder = new DelegateCommand(o => OrderConfirmDialog(), o =>CanOrder() );
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

    private bool CanOrder()
    {
        foreach (var item in DynamicInventory)
        {
            if (item.IsChecked &&  int.TryParse(item.OrderQuantityString, out int value))
            {
                item.OrderQuantity = value;
                if (value > 0)
                    return true;
            }
        }

        return false;
    }

    private void OrderConfirmDialog()
    {
        var vm = new DynamicEquipmentOrderConfirmViewModel(DynamicInventory.Where(item => item.IsChecked), _orderService);
        var confirmDialog = new DynamicOrderConfirmView { DataContext = vm };
        vm.OnRequestClose += (s, e) => confirmDialog.Close();
        confirmDialog.Show();
    }
}