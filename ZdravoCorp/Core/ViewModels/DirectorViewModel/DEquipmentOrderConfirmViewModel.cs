using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Equipment;
using ZdravoCorp.Core.Models.Orders;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.Order;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.CronJobs;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DEquipmentOrderConfirmViewModel

{
    private OrderRepository _orderRepository;
    public IEnumerable<DynamicInventoryViewModel> SelectedForOrder { get; }
    public ICommand ConfirmOrder { get; }
    public ICommand CancelOrder { get; }
    
    public event EventHandler? OnRequestClose;

    public DEquipmentOrderConfirmViewModel(IEnumerable<DynamicInventoryViewModel> selectedForOrder,
        OrderRepository orderRepository, InventoryRepository inventoryRepository)
    {
        _orderRepository = orderRepository;
        SelectedForOrder = selectedForOrder;
        ConfirmOrder = new DelegateCommand(o => Confirm());
        CancelOrder = new DelegateCommand(o => Cancel());
    }

    private void Cancel()
    {
        OnRequestClose?.Invoke(this, new EventArgs());
    }

    private Dictionary<int, int> InitOrder()
    {
        Dictionary<int, int> order = new Dictionary<int, int>();
        foreach (var item in SelectedForOrder)
        {
            order.Add(item.EquipmentId, item.OrderQuantity);
        }
        return order;
    }
    private void Confirm()
    {
       var order = InitOrder();
       Order newOrder = new Order(IDGenerator.GetId(), order, DateTime.Now, DateTime.Now.AddMinutes(5),
            Order.OrderStatus.Pending);
        _orderRepository.AddOrder(newOrder);
        Serializer.Save(_orderRepository);
        JobScheduler.DEquipmentTaskScheduler(newOrder);
        _orderRepository.OnRequestUpdate(this, new EventArgs());
        OnRequestClose?.Invoke(this, new EventArgs());
        
    }
}