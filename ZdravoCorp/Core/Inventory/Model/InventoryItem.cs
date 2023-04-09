using ZdravoCorp.Core.Rooms;
using ZdravoCorp.Core.Equipments.Model;
namespace ZdravoCorp.Core.Inventory;

public class InventoryItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public Room Room { get; set; }
    public Equipment Equipment { get; set; }
    public int EquipmentId { get; set; }
    
}