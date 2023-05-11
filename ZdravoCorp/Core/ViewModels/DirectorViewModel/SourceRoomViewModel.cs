using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Rooms;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class SourceRoomViewModel : ViewModelBase
{
    private InventoryItem _inventoryItem;
    public int ItemId => _inventoryItem.Id;
    public int Id => _inventoryItem.RoomId;
    public int Quantity => _inventoryItem.Quantity; 

    public SourceRoomViewModel(InventoryItem inventoryItem)
    {
        _inventoryItem = inventoryItem;
    }
}