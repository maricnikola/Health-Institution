using System;
using ZdravoCorp.Core.HospitalAssets.Rooms.Models;

namespace ZdravoCorp.Core.HospitalAssets.Equipment.Models;

public class TransferDTO
{
    public int Id { get; }
    public Room From { get; }
    public Room To { get; }
    public DateTime When { get; set; }
    public int Quantity { get; set; }

    public int InventoryId { get; set; }
    public string? InventoryItemName { get; set; }

    public Transfer.TransferStatus Status { get; set; }

    public TransferDTO(int id, Room from, Room to, DateTime when, int quantity, int inventoryId, string? inventoryItemName, Transfer.TransferStatus status)
    {
        Id = id;
        From = from;
        To = to;
        When = when;
        Quantity = quantity;
        InventoryId = inventoryId;
        InventoryItemName = inventoryItemName;
        Status = status;
    }
}