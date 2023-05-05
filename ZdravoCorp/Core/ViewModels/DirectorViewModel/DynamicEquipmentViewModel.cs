using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.View.DirectorView;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DynamicEquipmentViewModel : ViewModelBase
{
    private InventoryRepository _inventoryRepository;
    private ObservableCollection<DynamicInventoryViewModel> _dynamicInventory;
    public ICommand CreateOrder { get; }
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

        CreateOrder = new DelegateCommand(o=> OrderConfirmDialog());
    }
    private void OrderConfirmDialog()
    {
        var confirmDialog = new DEquipmentOrderConfirmView() { DataContext = new DEquipmentOrderConfirmViewModel(DynamicInventory.Where(item => item.IsChecked)) };
        confirmDialog.Show();
    }
}