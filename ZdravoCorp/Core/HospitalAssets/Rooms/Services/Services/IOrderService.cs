using System;
using System.Collections.Generic;
using ZdravoCorp.Core.HospitalAssets.Equipment.Models;

namespace ZdravoCorp.Core.HospitalAssets.Rooms.Services.Services;

public interface IOrderService
{
    public List<Order>? GetAll();
    public Order? GetById(int id);
    public void AddOrder(OrderDTO orderDto);
    public void UpdateStatus(int id, Order.OrderStatus status);

    public void Update(int id, OrderDTO orderDto);

    public void Delete(int id);

    public event EventHandler DataChanged;
}