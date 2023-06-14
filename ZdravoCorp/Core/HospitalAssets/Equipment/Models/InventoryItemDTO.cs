using ZdravoCorp.Core.HospitalAssets.Rooms.Models;

namespace ZdravoCorp.Core.HospitalAssets.Equipment.Models;

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
    public InventoryItemDTO(int id, int quantity, int room, Equipment? equipment)
    {
        Id = id;
        Quantity = quantity;
        Room.Id = room;
        Equipment = equipment;
    }
}