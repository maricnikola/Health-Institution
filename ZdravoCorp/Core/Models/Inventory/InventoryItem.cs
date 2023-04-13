namespace ZdravoCorp.Core.Models.Inventory;

public class InventoryItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public Room.Room Room { get; set; }
    public Equipment.Equipment Equipment { get; set; }
    public int EquipmentId { get; set; }
    
}