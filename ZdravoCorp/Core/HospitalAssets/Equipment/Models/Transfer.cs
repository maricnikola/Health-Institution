﻿using System;
using Newtonsoft.Json;
using ZdravoCorp.Core.HospitalAssets.Rooms.Models;

namespace ZdravoCorp.Core.HospitalAssets.Equipment.Models;

public class Transfer
{
    [JsonConstructor]
    public Transfer(int id, Room from, Room to, DateTime when, int quantity, int inventoryId, string? inventoryItemName)
    {
        InventoryId = inventoryId;
        Id = id;
        From = from;
        To = to;
        When = when;
        Quantity = quantity;
        InventoryItemName = inventoryItemName;
    }

    public Transfer(Room from, Room to, int quantity, int inventoryId)
    {
        From = from;
        To = to;
        Quantity = quantity;
        InventoryId = inventoryId;
    }

    public int Id { get; }
    public Room From { get; }
    public Room To { get; }
    public DateTime When { get; set; }
    public int Quantity { get; set; }

    public int InventoryId { get; set; }
    public string? InventoryItemName { get; set; }
    
    public TransferStatus Status { get; set; }
    
    public enum TransferStatus
    {
        Pending,
        Completed,
        Failed
    }
    
    public Transfer(TransferDTO transferDto)
    {
        From = transferDto.From;
        InventoryId = transferDto.InventoryId;
        Id = transferDto.Id;
        To = transferDto.To;
        InventoryItemName = transferDto.InventoryItemName;
        Quantity = transferDto.Quantity;
        When = transferDto.When;
        Status = transferDto.Status;
    }
}