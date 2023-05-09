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

public class EquipmentTransferWindowViewModel

{
    public IEnumerable<DynamicInventoryViewModel> SelectedForOrder { get; }
    public ICommand ConfirmOrder { get; }
    public ICommand CancelOrder { get; }
    private OrderRepository _orderRepository;
    private InventoryRepository _inventoryRepository;
    //private ObservableCollection<> _rooms;
    public event EventHandler OnRequestClose;
    public int InventoryItemId { get; set; }
    public EquipmentTransferWindowViewModel(IEnumerable<DynamicInventoryViewModel> selectedForOrder, OrderRepository orderRepository, InventoryRepository inventoryRepository)
    {
        _orderRepository = orderRepository;
        _inventoryRepository = inventoryRepository;
        SelectedForOrder = selectedForOrder;
        ConfirmOrder = new DelegateCommand(o => Confirm());
        CancelOrder = new DelegateCommand(o => Cancel());
    }

    public EquipmentTransferWindowViewModel(int inventoryItemId)
    {
        InventoryItemId = inventoryItemId;
    }
    private void Cancel()
    {
        OnRequestClose(this, new EventArgs());
    }
    private void Confirm()
    {
        Dictionary<int, int> order = new Dictionary<int, int>();
        foreach (var item in SelectedForOrder)
        {
            order.Add(item.EquipmentId, item.OrderQuantity);
        }

        Order newOrder = new Order(IDGenerator.GetId(), order, DateTime.Now, DateTime.Now.AddMinutes(1),
            Order.OrderStatus.Pending);
        _orderRepository.AddOrder(newOrder);
        JobScheduler.DEquipmentTaskScheduler(newOrder,_inventoryRepository );
        OnRequestClose(this, new EventArgs());
    }
}