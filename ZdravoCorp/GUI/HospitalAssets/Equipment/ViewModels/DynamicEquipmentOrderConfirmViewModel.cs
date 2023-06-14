using System;
using System.Collections.Generic;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalAssets.Equipment.Models;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.CronJobs;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalAssets.Equipment.ViewModels;

public class DynamicEquipmentOrderConfirmViewModel

{
    private readonly IOrderService _orderService;

    public DynamicEquipmentOrderConfirmViewModel(IEnumerable<DynamicInventoryViewModel> selectedForOrder,
        IOrderService orderService)
    {
        _orderService = orderService;
        SelectedForOrder = selectedForOrder;
        ConfirmOrder = new DelegateCommand(o => Confirm());
        CancelOrder = new DelegateCommand(o => Cancel());
    }

    public IEnumerable<DynamicInventoryViewModel> SelectedForOrder { get; }
    public ICommand ConfirmOrder { get; }
    public ICommand CancelOrder { get; }

    public event EventHandler? OnRequestClose;

    private void Cancel()
    {
        OnRequestClose?.Invoke(this, new EventArgs());
    }

    private Dictionary<int, int> InitOrder()
    {
        var order = new Dictionary<int, int>();
        foreach (var item in SelectedForOrder) order.Add(item.EquipmentId, item.OrderQuantity);
        return order;
    }

    private void Confirm()
    {
        var order = InitOrder();
        var newOrder = new OrderDTO(IDGenerator.GetId(), order, DateTime.Now, DateTime.Now.AddMinutes(5),
            Order.OrderStatus.Pending);
        _orderService.AddOrder(newOrder);
        JobScheduler.DEquipmentTaskScheduler(newOrder);
        OnRequestClose?.Invoke(this, new EventArgs());
    }
}