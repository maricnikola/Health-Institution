using System.Collections.Generic;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Transfers;

namespace ZdravoCorp.Core.Repositories.InventoryRepo;

public interface IInventoryRepository : IRepository<InventoryItem>
{
    List<InventoryItem> GetNonDynamic();
    List<InventoryItem>? GetDynamic();
    List<InventoryItem> GetDynamicGrouped();
    List<InventoryItem> LocateItem(InventoryItem inventoryItem);
    void UpdateInventoryItem(Transfer transfer);
    void UpdateDestinationInventoryItem(int source, int destination, int quantity);
    void LoadRoomsAndEquipment();
}