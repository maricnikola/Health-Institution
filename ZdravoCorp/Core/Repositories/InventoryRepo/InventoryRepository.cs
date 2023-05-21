using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Transfers;
using ZdravoCorp.Core.Repositories.EquipmentRepo;
using ZdravoCorp.Core.Repositories.RoomRepo;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.InventoryRepo;

public class InventoryRepository : ISerializable, IInventoryRepository
{
    private readonly EquipmentRepository _equipmentRepository;
    private readonly string _fileName = @".\..\..\..\Data\inventory.json";
    private readonly RoomRepository _roomRepository;
    private List<InventoryItem>? _inventory;


    public EventHandler OnRequestUpdate = null!;

    public InventoryRepository(RoomRepository roomRepository, EquipmentRepository equipmentRepository)
    {
        _roomRepository = roomRepository;
        _equipmentRepository = equipmentRepository;
        _inventory = new List<InventoryItem>();
        Serializer.Load(this);
        LoadRoomsAndEquipment();
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

    public void Insert(InventoryItem newInventoryItem)
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

        Serializer.Save(this);
    }


    public IEnumerable<InventoryItem> GetAll()
    {
        return _inventory;
    }


    public void Delete(InventoryItem entity)
    {
        _inventory.Remove(entity);
        Serializer.Save(this);
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


    public InventoryItem GetById(int id)
    {
        return _inventory.FirstOrDefault(inv => inv.Id == id);
    }

    public void SaveChanges()
    {
        Serializer.Save(this);
    }
}