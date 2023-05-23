using System.Collections.Generic;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Transfers;

namespace ZdravoCorp.Core.Repositories.InventoryRepo;

public interface IInventoryRepository : IRepository<InventoryItem>
{

    void LoadRoomsAndEquipment();
    void SaveChanges();
}