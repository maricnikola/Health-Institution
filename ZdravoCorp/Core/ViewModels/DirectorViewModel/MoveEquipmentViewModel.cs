using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Equipment;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.Room;
using ZdravoCorp.View.DirectorView;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class MoveEquipmentViewModel : ViewModelBase
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


    private void UpdateTable()
    {
        lock (_lock)
        {
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

    public MoveEquipmentViewModel(InventoryRepository inventoryRepository, RoomRepository roomRepository)
    {
        _lock = new object();
        _roomRepository = _roomRepository;
        _inventoryRepository = inventoryRepository;
        _inventoryRepository.OnRequestUpdate += (s, e) => UpdateTable();
        _allInventory = new ObservableCollection<InventoryViewModel>();
        MoveSelectedInventoryItem = new DelegateCommand(o => MoveInventoryItem(), o => IsInventoryItemSelected());
        foreach (var inventoryItem in _inventoryRepository.GetNonDynamic())
        {
            _allInventory.Add(new InventoryViewModel(inventoryItem));
        }

        _inventory = _allInventory;
        BindingOperations.EnableCollectionSynchronization(_inventory, _lock);
    }

    private void MoveInventoryItem()
    {
        var inventoryItemId = SelectedInventoryItemVm.Id;
        var vm = new EquipmentTransferWindowViewModel(inventoryItemId, _roomRepository, _inventoryRepository);
       
        var transferWindow = new EquipmentTransferWindowView()
            { DataContext = vm  };
        vm.OnRequestClose +=  (s, e) => transferWindow.Close();
        transferWindow.Show();
    }

    private bool IsInventoryItemSelected()
    {
        return SelectedInventoryItemVm != null;
    }
}