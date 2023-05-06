using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Equipment;
using ZdravoCorp.Core.Models.Order;
using ZdravoCorp.Core.Repositories.Order;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DEquipmentOrderConfirmViewModel

{
    //private ObservableCollection<DynamicInventoryViewModel> _selectedForOrder;
    public IEnumerable<DynamicInventoryViewModel> SelectedForOrder { get; }
    public ICommand ConfirmOrder { get; }
    public ICommand CancelOrder { get; }
    private OrderRepository _orderRepository;
    public event EventHandler OnRequestClose;
    public DEquipmentOrderConfirmViewModel(IEnumerable<DynamicInventoryViewModel> selectedForOrder, OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
        SelectedForOrder = selectedForOrder;
        ConfirmOrder = new DelegateCommand(o => Confirm());
        CancelOrder = new DelegateCommand(o => Cancel());
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
            order.Add(item.Id, item.OrderQuantity);
        }
        _orderRepository.AddOrder(new Order(IDGenerator.GetId(), order, DateTime.Now, DateTime.Now.AddDays(1)));
        OnRequestClose(this, new EventArgs());
    }
}