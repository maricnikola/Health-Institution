using System.Text.Json.Serialization;
using ZdravoCorp.Core.Repositories.Room;

namespace ZdravoCorp.Core.Models.Inventory;

public class InventoryItem
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
    [JsonIgnore] public Room.Room Room { get; set; }
    [JsonIgnore] public Equipment.Equipment Equipment { get; set; }

    [JsonPropertyName("equipment_id")] public int EquipmentId { get; } 
    [JsonPropertyName("room_id")] public int RoomId { get; } 
    
    
    public InventoryItem(int id, int quantity, Room.Room room, Equipment.Equipment equipment)
    {
        Id = id;
        Quantity = quantity;
        Room = room;
        Equipment = equipment;
        EquipmentId = equipment.Id;
        RoomId = room.Id;
    }
    [JsonConstructor]
    public InventoryItem(int id, int quantity, int roomid,int equipmentid)
    {
        Id = id;
        Quantity = quantity;
        EquipmentId = equipmentid;
        RoomId = roomid;
    }
}