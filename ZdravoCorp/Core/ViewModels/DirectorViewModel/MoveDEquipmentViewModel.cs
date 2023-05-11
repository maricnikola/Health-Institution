using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.Room;
using ZdravoCorp.Core.Repositories.Transfers;
using ZdravoCorp.View.DirectorView;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class MoveDEquipmentViewModel : ViewModelBase
{
    private InventoryRepository _inventoryRepository;
    private RoomRepository _roomRepository;
    private ObservableCollection<InventoryViewModel> _inventory;
    private ObservableCollection<InventoryViewModel> _allInventory;
    private object _lock;
    private string _searchText = "";
    public InventoryViewModel? SelectedInventoryItemVm { get; set; }
    

    public ICommand MoveSelectedInventoryItem { get; }
    
    

    public string SearchBox
    {
        get { return _searchText; }
        set
        {
            _searchText = value;
            UpdateTable();
            OnPropertyChanged("Search");
        }
    }


    private void UpdateTable(bool newAdded = false)
    {
        
        lock (_lock)
        {
            if (newAdded)
            {
                _allInventory = new ObservableCollection<InventoryViewModel>();
                foreach (var inventoryItem in _inventoryRepository.GetDynamic())
                {
                    if (inventoryItem.Quantity <= 5)
                        _allInventory.Add(new InventoryViewModel(inventoryItem));
                }
            }
            Inventory = UpdateTableFromSearch();
        }
    }

    private ObservableCollection<InventoryViewModel> UpdateTableFromSearch()
    {
        if (_searchText != "")
        {
            return new ObservableCollection<InventoryViewModel>(Search(_searchText));
        }
        else
        {
            return _allInventory;
        }
    }

    public IEnumerable<InventoryViewModel> Search(string inputText)
    {
        return _allInventory.Where(item => item.ToString().Contains(inputText));
    }


    public IEnumerable<InventoryViewModel> Inventory
    {
        get { return _inventory; }
        set
        {
            _inventory = new ObservableCollection<InventoryViewModel>(value);
            OnPropertyChanged();
        }
    }

    public MoveDEquipmentViewModel(InventoryRepository inventoryRepository, RoomRepository roomRepository)
    {
        _lock = new object();
        _roomRepository = roomRepository;
        _inventoryRepository = inventoryRepository;
        _inventoryRepository.OnRequestUpdate += (s, e) => UpdateTable(true);
        _allInventory = new ObservableCollection<InventoryViewModel>();
        MoveSelectedInventoryItem = new DelegateCommand(o => MoveInventoryItem(), o => IsInventoryItemSelected());
        foreach (var inventoryItem in _inventoryRepository.GetDynamic())
        {
            if (inventoryItem.Quantity <= 5)
                _allInventory.Add(new InventoryViewModel(inventoryItem));
        }

        _inventory = _allInventory;
        BindingOperations.EnableCollectionSynchronization(_inventory, _lock);
    }
    
    private void MoveInventoryItem()
    {
        if (SelectedInventoryItemVm != null)
        {
            var inventoryItemId = SelectedInventoryItemVm.Id;
            var roomId = SelectedInventoryItemVm.Room;
            var vm = new DEquipmentTransferWindowViewModel(inventoryItemId, roomId, SelectedInventoryItemVm.Quantity, _inventoryRepository);
       
            var transferWindow = new DynamicTransferWindowView()
                { DataContext = vm  };
            vm.OnRequestClose +=  (s, e) => transferWindow.Close();
            transferWindow.Show();
        }
    }

    private bool IsInventoryItemSelected()
    {
        return SelectedInventoryItemVm != null;
    }
}