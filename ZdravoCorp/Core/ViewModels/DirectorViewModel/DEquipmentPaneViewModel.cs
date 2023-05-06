using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.Order;
using ZdravoCorp.View.DirectorView;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DEquipmentPaneViewModel : ViewModelBase
{
    private InventoryRepository _inventoryRepository;
    private OrderRepository _orderRepository;
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
    public DEquipmentPaneViewModel(InventoryRepository inventoryRepository, OrderRepository orderRepository)
    {
        _dynamicInventory = new ObservableCollection<DynamicInventoryViewModel>();
        _inventoryRepository = inventoryRepository;
        _orderRepository = orderRepository;
        foreach (var inventoryItem in _inventoryRepository.GetDynamic())
        {
            if (inventoryItem.Quantity < 5)
                _dynamicInventory.Add(new DynamicInventoryViewModel(inventoryItem));
        }

        CreateOrder = new DelegateCommand(o=> OrderConfirmDialog());
    }
    private void OrderConfirmDialog()
    {
        var vm = new DEquipmentOrderConfirmViewModel(DynamicInventory.Where(item => item.IsChecked), _orderRepository);
        var confirmDialog = new DEquipmentOrderConfirmView() { DataContext = vm };
        vm.OnRequestClose += (s, e) => confirmDialog.Close();
        confirmDialog.Show();
    }
}