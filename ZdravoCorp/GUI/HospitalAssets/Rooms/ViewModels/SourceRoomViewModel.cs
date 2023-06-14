using ZdravoCorp.Core.HospitalAssets.Equipment.Models;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalAssets.Rooms.ViewModels;

public class SourceRoomViewModel : ViewModelBase
{
    private readonly InventoryItem _inventoryItem;

    public SourceRoomViewModel(InventoryItem inventoryItem)
    {
        _inventoryItem = inventoryItem;
    }

    public int ItemId => _inventoryItem.Id;
    public int Id => _inventoryItem.RoomId;
    public int Quantity => _inventoryItem.Quantity;
}