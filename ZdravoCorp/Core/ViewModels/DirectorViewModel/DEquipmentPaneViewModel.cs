using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
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
    private object _lock;
    public ICommand CreateOrder { get; }

    public IEnumerable<DynamicInventoryViewModel> DynamicInventory

    {
        get { return _dynamicInventory; }
        set
        {
            _dynamicInventory = new ObservableCollection<DynamicInventoryViewModel>(value);
            OnPropertyChanged();
        }
    }

    public DEquipmentPaneViewModel(InventoryRepository inventoryRepository, OrderRepository orderRepository)
    {
        _lock = new object();
        _dynamicInventory = new ObservableCollection<DynamicInventoryViewModel>();
        BindingOperations.EnableCollectionSynchronization(_dynamicInventory, _lock);
        _inventoryRepository = inventoryRepository;
        _orderRepository = orderRepository;
        _inventoryRepository.OnRequestUpdate += (s, e) => RefreshInventory();
        foreach (var inventoryItem in _inventoryRepository.GetDynamic())
        {
            if (inventoryItem.Quantity < 5)
                _dynamicInventory.Add(new DynamicInventoryViewModel(inventoryItem));
        }

        CreateOrder = new DelegateCommand(o => OrderConfirmDialog());
    }

    private void RefreshInventory()
    {
        lock (_lock)
        {
            var updateInventory = new ObservableCollection<DynamicInventoryViewModel>();
            foreach (var inventoryItem in _inventoryRepository.GetDynamic())
            {
                if (inventoryItem.Quantity < 5)
                    updateInventory.Add(new DynamicInventoryViewModel(inventoryItem));
            }

            DynamicInventory = updateInventory;
        }
    }

    private void OrderConfirmDialog()
    {
        var vm = new DEquipmentOrderConfirmViewModel(DynamicInventory.Where(item => item.IsChecked), _orderRepository,
            _inventoryRepository);
        var confirmDialog = new DEquipmentOrderConfirmView() { DataContext = vm };
        vm.OnRequestClose += (s, e) => confirmDialog.Close();
        confirmDialog.Show();
    }
}