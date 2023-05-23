using System;
using ZdravoCorp.Core.Models.Rooms;

namespace ZdravoCorp.Core.Models.Transfers;

public class TransferDTO
{
    public int Id { get; }
    public Room From { get; }
    public Room To { get; }
    public DateTime When { get; set; }
    public int Quantity { get; set; }

    public int InventoryId { get; set; }
    public string? InventoryItemName { get; set; }


    public TransferDTO(int id, Room from, Room to, DateTime when, int quantity, int inventoryId, string? inventoryItemName)
    {
        Id = id;
        From = from;
        To = to;
        When = when;
        Quantity = quantity;
        InventoryId = inventoryId;
        InventoryItemName = inventoryItemName;
    }
}