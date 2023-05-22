using System;
using System.Collections.Generic;
using ZdravoCorp.Core.Models.Orders;

namespace ZdravoCorp.Core.Services.OrderServices;

public interface IOrderService
{
    public event EventHandler OnRequestUpdate;
    public List<Order>? GetAll();
    public Order? GetById(int id);
    public void AddOrder(OrderDTO orderDto);
    public void UpdateStatus(int id, Order.OrderStatus status);

    public void Update(int id, OrderDTO orderDto);

    public void Delete(int id);
}