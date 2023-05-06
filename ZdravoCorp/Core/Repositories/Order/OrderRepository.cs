using System.Collections.Generic;

namespace ZdravoCorp.Core.Repositories.Order;

public class OrderRepository
{
    private List<Models.Order.Order> _orders;
    private readonly string _fileName = @".\..\..\..\Data\orders.json";
    
    public OrderRepository()
    {
        _orders = new List<Models.Order.Order>();
    }

    public void AddOrder(Models.Order.Order order)
    {
        _orders.Add(order);
    }

    public List<Models.Order.Order> GetOrders()
    {
        return _orders;
    }
}