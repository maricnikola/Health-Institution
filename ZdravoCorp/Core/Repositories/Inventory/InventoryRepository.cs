using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Models.Equipment;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Room;
using ZdravoCorp.Core.Repositories.Equipment;
using ZdravoCorp.Core.Repositories.Room;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.Inventory;

public class InventoryRepository : ISerializable
{
    private readonly RoomRepository _roomRepository;
    private readonly EquipmentRepository _equipmentRepository;
    private  List<InventoryItem>? _inventory;
    private readonly string _fileName = @".\..\..\..\Data\inventory.json";



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
        Serializer.Load(this);
        LoadRoomsAndEquipment();
    }

    public List<InventoryItem> GetNonDynamic()
    {
        return _inventory.Where(item => item.Equipment.IsDynamic == false).ToList();
    }

    public List<InventoryItem>? GetAll()
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

   
    
    public InventoryItem? GetInventoryById(int id)
    {
        return _inventory.FirstOrDefault(inv => inv.Id == id);
    }

    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _inventory;
    }

    public void Import(JToken token)
    {
        _inventory = token.ToObject<List<InventoryItem>>();
    }
}