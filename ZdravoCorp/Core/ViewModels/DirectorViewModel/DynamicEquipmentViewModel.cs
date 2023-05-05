using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZdravoCorp.Core.Repositories.Inventory;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DynamicEquipmentViewModel : ViewModelBase
{
    private InventoryRepository _inventoryRepository;
    private ObservableCollection<DynamicInventoryViewModel> _dynamicInventory;

    public IEnumerable<DynamicInventoryViewModel> DynamicInventory
    {
        get
        {
            return _dynamicInventory;
        }
        set
        {
            _dynamicInventory = new ObservableCollection<DynamicInventoryViewModel>(value);
            OnPropertyChanged();
        }
    }
    public DynamicEquipmentViewModel(InventoryRepository inventoryRepository)
    {
        _dynamicInventory = new ObservableCollection<DynamicInventoryViewModel>();
        _inventoryRepository = inventoryRepository;
        foreach (var inventoryItem in _inventoryRepository.GetDynamic())
        {
            _dynamicInventory.Add(new DynamicInventoryViewModel(inventoryItem));
        }
    }
}