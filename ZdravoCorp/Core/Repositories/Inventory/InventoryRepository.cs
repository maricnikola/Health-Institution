using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Core.Models.Equipment;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Room;

namespace ZdravoCorp.Core.Repositories.Inventory;

public class InventoryRepository
{
    private List<InventoryItem> inventory { get; set; }
    private string _fileName = "";

    public void AddItem(InventoryItem newInventoryItem)
    {
        inventory.Add(newInventoryItem);
    }
    public void LoadFromFile()
    {
        
    }

    public List<InventoryItem> FilterByRoomType(RoomType roomType)
    {
        return inventory.Where(item => item.Room.Type == roomType).ToList();
    }

    public List<InventoryItem> FilterRoomByEquipmentType(EquipmentType equipmentType)
    {
        return inventory.Where(item => item.Equipment.Type == equipmentType).ToList();
    }

    public List<InventoryItem> FilterByQuantity(int quantity)
    {
        return inventory.Where(item => item.Quantity <= quantity).ToList();
    }

    public List<InventoryItem> Search(string inputText)
    {
        return inventory.Where(item => item.Equipment.ToString().Contains(inputText)).ToList();
    }
    public List<InventoryItem> GetAll()
    {
        return inventory;
    }
}