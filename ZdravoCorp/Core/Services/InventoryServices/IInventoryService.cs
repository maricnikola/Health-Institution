using System;
using System.Collections.Generic;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Transfers;

namespace ZdravoCorp.Core.Services.InventoryServices;

public interface IInventoryService
{
    public List<InventoryItem>? GetAll();
    public InventoryItem? GetById(int id);
    public event EventHandler DataChanged;
    public void AddInventoryItem(InventoryItemDTO inventoryItemDto);
    public void AddFromOrder(InventoryItem inventoryItem);

    public void Update(int id, InventoryItemDTO inventoryItemDto);

    public void Delete(int id);
    
    public List<InventoryItem> LocateItemInOtherRooms(InventoryItem inventoryItem);
    List<InventoryItem> GetNonDynamic();
    List<InventoryItem> GetDynamic();
    List<InventoryItem> GetDynamicGrouped();
    
    bool UpdateInventoryItem(Transfer transfer);
    void UpdateDestinationInventoryItem(int source, int destination, int quantity);
    



}