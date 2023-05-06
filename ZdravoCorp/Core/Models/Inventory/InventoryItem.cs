using System;
using System.Text.Json.Serialization;
using ZdravoCorp.Core.Repositories.Room;

namespace ZdravoCorp.Core.Models.Inventory;

public class InventoryItem
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
    [JsonIgnore] public Room.Room? Room { get; set; }
    [JsonIgnore] public Equipment.Equipment? Equipment { get; set; }

    [JsonPropertyName("equipment_id")] public int EquipmentId { get; } 
    [JsonPropertyName("room_id")] public int RoomId { get; } 
    
    
    public InventoryItem(int id, int quantity, Room.Room? room, Equipment.Equipment? equipment)
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

    public InventoryItem(InventoryItem other)
    {
        Id = other.Id;
        Quantity = other.Quantity;
        Room = other.Room;
        Equipment = other.Equipment;
        EquipmentId = other.EquipmentId;
        RoomId = other.RoomId;
    }

    protected bool Equals(InventoryItem other)
    {
        return Id == other.Id && Quantity == other.Quantity && Equals(Room, other.Room) && Equals(Equipment, other.Equipment) && EquipmentId == other.EquipmentId && RoomId == other.RoomId;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((InventoryItem)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Quantity, Room, Equipment, EquipmentId, RoomId);
    }
}