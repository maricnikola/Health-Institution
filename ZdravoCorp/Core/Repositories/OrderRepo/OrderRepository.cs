using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Orders;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.OrderRepo;

public class OrderRepository : ISerializable, IOrderRepository
{
    private readonly string _fileName = @".\..\..\..\Data\orders.json";
    private List<Order>? _orders;
    

    public OrderRepository()
    {
        _orders = new List<Order>();
        Serializer.Load(this);
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
        _orders = token.ToObject<List<Order>>();
    }

    public void Insert(Order order)
    {
        _orders.Add(order);
        Serializer.Save(this);
    }

    public IEnumerable<Order> GetAll()
    {
        return _orders;
    }


    public void Delete(Order entity)
    {
        _orders.Remove(entity);
        Serializer.Save(this);
    }

    public Order GetById(int id)
    {
        return _orders.FirstOrDefault(order => order.Id == id);
    }
}