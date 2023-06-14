using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.HospitalAssets.Equipment.Models;

public class Order
{
    public enum OrderStatus
    {
        Pending,
        Completed
    }

    [JsonConstructor]
    public Order(int id, Dictionary<int, int> items, DateTime orderTime, DateTime arrivalTime, OrderStatus status)
    {
        Id = id;
        Items = items;
        OrderTime = orderTime;
        ArrivalTime = arrivalTime;
        Status = status;
    }

    public Order(OrderDTO orderDto)
    {
        Id=orderDto.Id;
        Items = orderDto.Items;
        OrderTime = orderDto.OrderTime;
        ArrivalTime = orderDto.ArrivalTime;
        Status = orderDto.Status;
    }

    public int Id { get; set; }
    public Dictionary<int, int> Items { get; set; }
    public DateTime OrderTime { get; set; }
    public DateTime ArrivalTime { get; set; }

    public OrderStatus Status { get; set; }
}