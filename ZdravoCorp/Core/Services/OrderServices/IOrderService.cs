using System.Collections.Generic;
using ZdravoCorp.Core.Models.Orders;

namespace ZdravoCorp.Core.Services.OrderServices;

public interface IOrderService
{
    public List<Order>? GetAll();
    public Order? GetById(int id);
    public void AddOrder(OrderDTO orderDto);

    public void Update(int id, OrderDTO orderDto);

    public void Delete(int id);
}