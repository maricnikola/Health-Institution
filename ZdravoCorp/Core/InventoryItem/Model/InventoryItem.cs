using ZdravoCorp.Core.Room;
namespace ZdravoCorp.Core.Inventory;

public class InventoryItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public Room.Room Room { get; set; }
    public Equipment.Model.Equipment Equipment { get; set; }
    public int EquipmentId { get; set; }
    
}