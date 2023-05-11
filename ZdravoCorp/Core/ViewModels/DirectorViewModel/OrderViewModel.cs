using System;
using System.Collections.Generic;
using ZdravoCorp.Core.Models.Orders;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class OrderViewModel : ViewModelBase
{
    private Order _order;
    public int Id => _order.Id;

    public string Items { get; set; }
    public string OrderTime => _order.OrderTime.ToString();
    public string ArrivalTime => _order.ArrivalTime.ToString();
    public string Status => _order.Status.ToString();

    
    public OrderViewModel(Order order, string items)
    {
        _order = order;
        Items = items;
    }
}