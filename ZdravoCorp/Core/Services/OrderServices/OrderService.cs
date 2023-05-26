using System;
using System.Collections.Generic;
using ZdravoCorp.Core.Models.Orders;
using ZdravoCorp.Core.Repositories.OrderRepo;

namespace ZdravoCorp.Core.Services.OrderServices;

public class OrderService : IOrderService
{
    private IOrderRepository _orderRepository;
    public event EventHandler OnRequestUpdate = null!;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public List<Order>? GetAll()
    {
        return (List<Order>?)_orderRepository.GetAll();
    }

    public Order? GetById(int id)
    {
        return _orderRepository.GetById(id);
        
    }

    public void AddOrder(OrderDTO orderDto)
    {
        _orderRepository.Insert(new Order(orderDto));
        DataChanged?.Invoke(this, new EventArgs());
    }

    public void UpdateStatus(int id, Order.OrderStatus status)
    {
        _orderRepository.UpdateStatus(id, status);
        DataChanged?.Invoke(this, new EventArgs());
    }

    public void Update(int id, OrderDTO orderDto)
    {
        var oldOrder = _orderRepository.GetById(id);
        if (oldOrder == null)
        {
            throw new KeyNotFoundException();
        }
        _orderRepository.Delete(oldOrder);
        _orderRepository.Insert(new Order(orderDto));
        DataChanged?.Invoke(this, new EventArgs());
    }

   

    public void Delete(int id)
    {
        _orderRepository.Delete(GetById(id));
        DataChanged?.Invoke(this, new EventArgs());
    }

    public event EventHandler? DataChanged;
}