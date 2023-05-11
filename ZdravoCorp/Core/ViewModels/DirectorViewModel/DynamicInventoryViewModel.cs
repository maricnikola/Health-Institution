using ZdravoCorp.Core.Models.Inventory;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DynamicInventoryViewModel : ViewModelBase
{
    private InventoryItem _inventoryItem;
    public int Id => _inventoryItem.Id;
    public int EquipmentId => _inventoryItem.EquipmentId;
    public string Name => _inventoryItem.Equipment.Name;
    public int Quantity => _inventoryItem.Quantity;
    public bool IsChecked { get; set; }
    public int OrderQuantity { get; set; }


    public DynamicInventoryViewModel(InventoryItem inventoryItem)
    {
        _inventoryItem = inventoryItem;
        IsChecked = false;
        OrderQuantity = 0;
    }
}