using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Transfers;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Services.InventoryServices;

public class InventoryService : IInventoryService
{

    private IInventoryRepository _inventoryRepository;


    public InventoryService(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }
    public List<InventoryItem>? GetAll()
    {
        return _inventoryRepository.GetAll() as List<InventoryItem>;
    }

    public InventoryItem? GetById(int id)
    {
        return _inventoryRepository.GetById(id);
    }

    public event EventHandler? DataChanged;

    public void AddInventoryItem(InventoryItemDTO inventoryItemDto)
    {
        _inventoryRepository.Insert(new InventoryItem(inventoryItemDto));
        DataChanged?.Invoke(this, new EventArgs());
    }

    public void Update(int id, InventoryItemDTO inventoryItemDto)
    {
        var oldInventoryItem = _inventoryRepository.GetById(id);
        if (oldInventoryItem == null)
        {
            throw new KeyNotFoundException();
        }

        _inventoryRepository.Delete(oldInventoryItem);
        _inventoryRepository.Insert(new InventoryItem(inventoryItemDto));
        DataChanged?.Invoke(this, new EventArgs());
    }
    

    public void Delete(int id)
    {
        _inventoryRepository.Delete(_inventoryRepository.GetById(id));
        DataChanged?.Invoke(this, new EventArgs());
    }
    

    public List<InventoryItem> LocateItemInOtherRooms(InventoryItem inventoryItem)
    {
        return _inventoryRepository.GetAll().Where(item =>
                item.EquipmentId == inventoryItem.EquipmentId && item.Quantity != 0 && item.Id != inventoryItem.Id)
            .ToList();
    }

    public void AddFromOrder(InventoryItem inventoryItem)
    {
        _inventoryRepository.Insert(inventoryItem);
        _inventoryRepository.LoadRoomsAndEquipment();
        DataChanged?.Invoke(this, new EventArgs());
    }

    public List<InventoryItem> GetNonDynamic()
    {
        return _inventoryRepository.GetAll().Where(item => item.Equipment != null && item.Equipment.IsDynamic == false).ToList();
    }

    public List<InventoryItem> GetDynamic()
    {
        return _inventoryRepository.GetAll().Where(item => item.Equipment != null && item.Equipment.IsDynamic).ToList();
    }

    public List<InventoryItem> GetDynamicGrouped()
    {
        var dynamicEquipment = new List<InventoryItem>();
        foreach (var inventoryItem in _inventoryRepository.GetAll())
            if (inventoryItem.Equipment != null && inventoryItem.Equipment.IsDynamic)
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

        return dynamicEquipment;
    }

    public bool UpdateInventoryItem(Transfer transfer)
    {
        var old = _inventoryRepository.GetAll().FirstOrDefault(item => item.Id == transfer.InventoryId);
        if (old == null)
        {
            throw new KeyNotFoundException();
        }

        if (old.Quantity < transfer.Quantity)
            return false;
        var newInventoryItem = new InventoryItem(old);
        newInventoryItem.Id = IDGenerator.GetId();
        old.Quantity -= transfer.Quantity;
        newInventoryItem.Quantity = transfer.Quantity;
        newInventoryItem.Room = transfer.To;
        newInventoryItem.RoomId = transfer.To.Id;
        _inventoryRepository.Insert(newInventoryItem);
        DataChanged?.Invoke(this, new EventArgs());
        return true;
    }

    public void UpdateDestinationInventoryItem(int source, int destination, int quantity)
    {
        var destinationItem = _inventoryRepository.GetAll().FirstOrDefault(item => item.Id == destination);
        var sourceItem = _inventoryRepository.GetAll().FirstOrDefault(item => item.Id == source);
        if (destinationItem == null || sourceItem == null)
        {
            throw new KeyNotFoundException();
        }
        destinationItem.Quantity += quantity;
        sourceItem.Quantity -= quantity;
        DataChanged?.Invoke(this, new EventArgs());
    }

    public void MoveItemsToStockRoom(int roomId)
    {
        List<int> removeIds = new List<int>();
        foreach (var item in _inventoryRepository.GetAll())
        {
            if (item.RoomId == roomId)
            {
                removeIds.Add(item.Id);
                var newItem = new InventoryItemDTO(IDGenerator.GetId(), item.Quantity, 999, item.Equipment);
                AddInventoryItem(newItem);
            }
        }

        foreach (var id in removeIds)
        {
            Delete(id);
        }
    }
}