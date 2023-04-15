using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Core.Models.Equipment;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Room;
using ZdravoCorp.Core.Repositories.Equipment;
using ZdravoCorp.Core.Repositories.Room;

namespace ZdravoCorp.Core.Repositories.Inventory;

public class InventoryRepository
{
    private RoomRepository _roomRepository;
    private EquipmentRepository _equipmentRepository;
    private List<InventoryItem> _inventory { get; set; }
    private string _fileName = @".\..\..\..\Data\inventory.json";

    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public void AddItem(InventoryItem newInventoryItem)
    {
        _inventory.Add(newInventoryItem);
    }

    public InventoryRepository(RoomRepository roomRepository, EquipmentRepository equipmentRepository)
    {
        _roomRepository = roomRepository;
        _equipmentRepository = equipmentRepository;
        _inventory = new List<InventoryItem>();
        LoadFromFile();
        SaveToFile();
    }


    public List<InventoryItem> FilterByRoomType(RoomType roomType)
    {
        return _inventory.Where(item => item.Room.Type == roomType).ToList();
    }

    public List<InventoryItem> FilterRoomByEquipmentType(Models.Equipment.Equipment.EquipmentType equipmentType)
    {
        return _inventory.Where(item => item.Equipment.Type == equipmentType).ToList();
    }

    public List<InventoryItem> FilterByQuantity(int quantity)
    {
        return _inventory.Where(item => item.Quantity <= quantity).ToList();
    }

    public List<InventoryItem> Search(string inputText)
    {
        return _inventory.Where(item => item.Equipment.ToString().Contains(inputText)).ToList();
    }

    public void SaveToFile()
    {
        var inventory = JsonSerializer.Serialize(_inventory, _serializerOptions);

        File.WriteAllText(this._fileName, inventory);
    }

    public void LoadFromFile()
    {
        var text = File.ReadAllText(_fileName);
        if (text == "")
            return;
        var inventory = JsonSerializer.Deserialize<List<InventoryItem>>(text);
        inventory.ForEach(inventory => _inventory.Add(inventory));
    }
}