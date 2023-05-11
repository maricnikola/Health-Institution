using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.Order;

public class OrderRepository : ISerializable
{
    private List<Models.Orders.Order>? _orders;
    private readonly string _fileName = @".\..\..\..\Data\orders.json";
    public  EventHandler OnRequestUpdate;
    public OrderRepository()
    {
        _orders = new List<Models.Orders.Order>();
        Serializer.Load(this);
    }

    public void AddOrder(Models.Orders.Order order)
    {
        _orders.Add(order);
    }

    public List<Models.Orders.Order>? GetOrders()
    {
        return _orders;
    }

    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _orders;
    }

    public void Import(JToken token)
    {
        _orders = token.ToObject<List<Models.Orders.Order>>();
    }
}