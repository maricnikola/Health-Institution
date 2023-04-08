using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using ZdravoCorp.Core.Equipment.Model;
using ZdravoCorp.Core.Room;

namespace ZdravoCorp.Core.Inventory.Repository;

public class InventoryRepository
{
    private List<InventoryItem> Inventory { get; set; }
    private string _fileName = "";

    public void AddItem(InventoryItem newInventoryItem)
    {
        Inventory.Add(newInventoryItem);
    }
    public void LoadFromFile()
    {
        
    }

    public List<InventoryItem> FilterByRoomType(RoomType roomType)
    {
        return Inventory.Where(item => item.Room.Type == roomType).ToList();
    }

    public List<InventoryItem> FilterRoomByEquipmentType(EquipmentType equipmentType)
    {
        return Inventory.Where(item => item.Equipment.Type == equipmentType).ToList();
    }

    public List<InventoryItem> FilterByQuantity(int quantity)
    {
        return Inventory.Where(item => item.Quantity <= quantity).ToList();
    }

    public List<InventoryItem> Search(string inputText)
    {
        return Inventory.Where(item => item.Equipment.ToString().Contains(inputText)).ToList();
    }
    public List<InventoryItem> GetAll()
    {
        return Inventory;
    }
}