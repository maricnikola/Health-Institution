using System;
using System.Collections.Generic;

namespace ZdravoCorp.Core.HospitalAssets.Equipment.Models;

public class OrderDTO
{
    public int Id { get; set; }
    public Dictionary<int, int> Items { get; set; }
    public DateTime OrderTime { get; set; }
    public DateTime ArrivalTime { get; set; }

    public Order.OrderStatus Status { get; set; }


    public OrderDTO(int id, Dictionary<int, int> items, DateTime orderTime, DateTime arrivalTime, Order.OrderStatus status)
    {
        Id = id;
        Items = items;
        OrderTime = orderTime;
        ArrivalTime = arrivalTime;
        Status = status;
    }
}