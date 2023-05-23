using System.ComponentModel;
using ZdravoCorp.Core.Models.Inventory;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DynamicInventoryViewModel : ViewModelBase, IDataErrorInfo
{
    private readonly InventoryItem _inventoryItem;


    public DynamicInventoryViewModel(InventoryItem inventoryItem)
    {
        _inventoryItem = inventoryItem;
        IsChecked = false;
        OrderQuantity = 0;
    }

    public int Id => _inventoryItem.Id;
    public int EquipmentId => _inventoryItem.EquipmentId;
    public string? Name => _inventoryItem.Equipment?.Name;
    public int Quantity => _inventoryItem.Quantity;
    public bool IsChecked { get; set; }
    public int OrderQuantity { get; set; }
    public string OrderQuantityString { get; set; }

    public string Error { get; }

    public string this[string columnName]
    {
        get
        {
            if (columnName == "OrderQuantityString")
            {
                if (string.IsNullOrEmpty(OrderQuantityString) || !int.TryParse(OrderQuantityString, out int value))
                    
                    return "Error";
            }

            return null;
        }
        
    }
}