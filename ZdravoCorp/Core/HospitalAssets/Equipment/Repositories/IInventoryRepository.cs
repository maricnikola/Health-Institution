using ZdravoCorp.Core.HospitalAssets.Equipment.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalAssets.Equipment.Repositories;

public interface IInventoryRepository : IRepository<InventoryItem>
{

    void LoadRoomsAndEquipment();
    void SaveChanges();
    void ChangeQuantity(InventoryItem inventoryItem, int quantity);
}