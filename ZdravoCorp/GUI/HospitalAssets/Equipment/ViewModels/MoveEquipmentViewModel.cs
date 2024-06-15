using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services.Services;
using ZdravoCorp.GUI.HospitalAssets.Equipment.Views;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalAssets.Equipment.ViewModels;

public class MoveEquipmentViewModel : ViewModelBase
{
    private  ObservableCollection<InventoryViewModel> _allInventory;
    private ObservableCollection<InventoryViewModel> _inventory;
    private readonly IInventoryService _inventoryService;
    private readonly object _lock;
    private readonly object _lock2;
    private readonly IRoomService _roomService;
    private string _searchText = "";
    private InventoryViewModel? _selectedInventoryItemVm;
    private readonly ITransferService _transferService;
    private ObservableCollection<TransferViewModel> _transfers;

    public MoveEquipmentViewModel(IInventoryService inventoryService, IRoomService roomService,
       ITransferService transferService)
    {
        _lock = new object();
        _lock2 = new object();
        _inventoryService = inventoryService;
        _roomService = roomService;
        _transferService = transferService;
        _inventoryService.DataChanged += (s, e) => UpdateTable(true);
        _transferService.DataChanged += (s, e) => UpdateTransfers();
        _allInventory = new ObservableCollection<InventoryViewModel>();
        _transfers = new ObservableCollection<TransferViewModel>();
        MoveSelectedInventoryItem = new DelegateCommand(o => MoveInventoryItem(), o => IsInventoryItemSelected());
        foreach (var inventoryItem in _inventoryService.GetNonDynamic())
            _allInventory.Add(new InventoryViewModel(inventoryItem));

        _inventory = _allInventory;
        UpdateTransfers();
        BindingOperations.EnableCollectionSynchronization(_inventory, _lock);
        BindingOperations.EnableCollectionSynchronization(_transfers, _lock2);
    }

    public InventoryViewModel? SelectedInventoryItemVm
    {
        get => _selectedInventoryItemVm;
        set
        {
            _selectedInventoryItemVm = value;
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public IEnumerable<TransferViewModel> Transfers
    {
        get => _transfers;
        set
        {
            _transfers = new ObservableCollection<TransferViewModel>(value);
            OnPropertyChanged();
        }
    }

    public ICommand MoveSelectedInventoryItem { get; }


    public string SearchBox
    {
        get => _searchText;
        set
        {
            _searchText = value;
            UpdateTable();
            OnPropertyChanged("Search");
        }
    }


    public IEnumerable<InventoryViewModel> Inventory
    {
        get => _inventory;
        set
        {
            _inventory = new ObservableCollection<InventoryViewModel>(value);
            OnPropertyChanged();
        }
    }


    private void UpdateTable(bool newAdded = false)
    {
        lock (_lock)
        {
            if (newAdded)
            {
                _allInventory = new ObservableCollection<InventoryViewModel>();
                foreach (var inventoryItem in _inventoryService.GetNonDynamic())
                    _allInventory.Add(new InventoryViewModel(inventoryItem));
            }

            Inventory = UpdateTableFromSearch();
        }
    }

    private ObservableCollection<InventoryViewModel> UpdateTableFromSearch()
    {
        if (_searchText != "")
            return new ObservableCollection<InventoryViewModel>(Search(_searchText));
        return _allInventory;
    }

    public IEnumerable<InventoryViewModel> Search(string inputText)
    {
        return _allInventory.Where(item => item.ToString().Contains(inputText));
    }

    private void UpdateTransfers()
    {
        lock (_lock2)
        {
            _transfers = new ObservableCollection<TransferViewModel>();
            foreach (var transfer in _transferService.GetAll()) _transfers.Add(new TransferViewModel(transfer));

            Transfers = _transfers;
        }
    }

    private void MoveInventoryItem()
    {
        if (SelectedInventoryItemVm != null)
        {
            var inventoryItemId = SelectedInventoryItemVm.Id;
            var roomId = SelectedInventoryItemVm.Room;
            var vm = new EquipmentTransferWindowViewModel(inventoryItemId, roomId, SelectedInventoryItemVm.Quantity,
                _roomService, _inventoryService, _transferService);

            var transferWindow = new EquipmentTransferWindowView { DataContext = vm };
            vm.OnRequestClose += (s, e) => transferWindow.Close();
            transferWindow.Show();
        }
    }

    private bool IsInventoryItemSelected()
    {
        return SelectedInventoryItemVm != null;
    }
}