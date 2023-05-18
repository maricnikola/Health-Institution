using ZdravoCorp.Core.Models.Equipments;
using ZdravoCorp.Core.Models.Rooms;

namespace ZdravoCorp.Core.Models.Inventory;

public class InventoryItemDTO
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public Room? Room { get; set; }
    public Equipment? Equipment { get; set; }


    public InventoryItemDTO(int id, int quantity, Room? room, Equipment? equipment)
    {
        Id = id;
        Quantity = quantity;
        Room = room;
        Equipment = equipment;
    }
}