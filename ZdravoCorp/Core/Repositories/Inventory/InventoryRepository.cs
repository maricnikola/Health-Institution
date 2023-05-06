using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Models.Equipment;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Room;
using ZdravoCorp.Core.Repositories.Equipment;
using ZdravoCorp.Core.Repositories.Room;

namespace ZdravoCorp.Core.Repositories.Inventory;

public class InventoryRepository
{
    private readonly RoomRepository _roomRepository;
    private readonly EquipmentRepository _equipmentRepository;
    private readonly List<InventoryItem> _inventory;
    private readonly string _fileName = @".\..\..\..\Data\inventory.json";

    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public EventHandler OnRequestUpdate;

    public void AddItem(InventoryItem newInventoryItem)
    {
        var index = _inventory.FindIndex(item =>
            item.EquipmentId == newInventoryItem.EquipmentId && item.RoomId == newInventoryItem.RoomId);
        if (index != -1)
        {
            var oldQuantity = _inventory.ElementAt(index).Quantity;
            _inventory.RemoveAt(index);
            newInventoryItem.Quantity += oldQuantity;
            _inventory.Add(newInventoryItem);
        }
        else
        {
            _inventory.Add(newInventoryItem);
        }
    }

    public InventoryRepository(RoomRepository roomRepository, EquipmentRepository equipmentRepository)
    {
        _roomRepository = roomRepository;
        _equipmentRepository = equipmentRepository;
        _inventory = new List<InventoryItem>();
        LoadFromFile();
        LoadRoomsAndEquipment();
    }

    public List<InventoryItem> GetNonDynamic()
    {
        return _inventory.Where(item => item.Equipment.IsDynamic == false).ToList();
    }

    public List<InventoryItem> GetAll()
    {
        return _inventory;
    }

    public List<InventoryItem> GetDynamic()
    {
        var dynamicEquipment = new List<InventoryItem>();
        foreach (var inventoryItem in _inventory)
        {
            if (inventoryItem.Equipment.IsDynamic)
            {
                var index = dynamicEquipment.FindIndex(item =>
                    item.EquipmentId == inventoryItem.EquipmentId);
                if (index != -1)
                {
                    dynamicEquipment.ElementAt(index).Quantity += inventoryItem.Quantity;
                }
                else
                {
                    var itemCopy = new InventoryItem(inventoryItem);
                    dynamicEquipment.Add(itemCopy);
                }
            }
        }

        return dynamicEquipment;
    }

    public void LoadRoomsAndEquipment()
    {
        foreach (var inventoryItem in _inventory)
        {
            if (inventoryItem.Room == null)
                inventoryItem.Room = _roomRepository.GetById(inventoryItem.RoomId);
            if (inventoryItem.Equipment == null)
                inventoryItem.Equipment = _equipmentRepository.GetById(inventoryItem.EquipmentId);
        }
    }

    public List<InventoryItem> FilterByRoomType(RoomType roomType)
    {
        return _inventory.Where(item => item.Room != null && item.Room.Type == roomType).ToList();
    }

    public List<InventoryItem> FilterRoomByEquipmentType(Models.Equipment.Equipment.EquipmentType equipmentType)
    {
        return _inventory.Where(item => item.Equipment != null && item.Equipment.Type == equipmentType).ToList();
    }

    public List<InventoryItem> FilterByQuantity(int quantity)
    {
        return _inventory.Where(item => item.Quantity <= quantity).ToList();
    }

    public List<InventoryItem> Search(string inputText)
    {
        return _inventory.Where(item => item.Equipment != null && item.Equipment.ToString().Contains(inputText))
            .ToList();
    }

    public void SaveToFile()
    {
        if (_inventory.Count == 0)
        {
            Trace.WriteLine($"Repository is empty! {this.GetType()}");
            return;
        }

        var inventory = JsonSerializer.Serialize(_inventory, _serializerOptions);

        File.WriteAllText(this._fileName, inventory);
    }

    public void LoadFromFile()
    {
        var text = File.ReadAllText(_fileName);
        if (text == "")
            throw new EmptyFileException("File is empty!");
        try
        {
            var inventory = JsonSerializer.Deserialize<List<InventoryItem>>(text);
            inventory?.ForEach(inv => _inventory.Add(inv));
        }
        catch (JsonException e)
        {
            Trace.WriteLine(e);
        }
    }

    public InventoryItem? GetInventoryById(int id)
    {
        return _inventory.FirstOrDefault(inv => inv.Id == id);
    }
}