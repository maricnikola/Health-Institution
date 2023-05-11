using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Equipment;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.Room;
using ZdravoCorp.Core.Repositories.Transfers;
using ZdravoCorp.View.DirectorView;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class MoveEquipmentViewModel : ViewModelBase
{
    private InventoryRepository _inventoryRepository;
    private TransferRepository _transferRepository;
    private RoomRepository _roomRepository;
    private ObservableCollection<InventoryViewModel> _inventory;
    private ObservableCollection<InventoryViewModel> _allInventory;
    private ObservableCollection<TransferViewModel> _transfers;
    private object _lock;
    private object _lock2;
    private string _searchText = "";
    private InventoryViewModel? _selectedInventoryItemVm;

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
        get
        {
            return _transfers;
        }
        set
        {
            _transfers = new ObservableCollection<TransferViewModel>(value);
            OnPropertyChanged();
        }
    }

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
                foreach (var inventoryItem in _inventoryRepository.GetNonDynamic())
                {
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

    public MoveEquipmentViewModel(InventoryRepository inventoryRepository, RoomRepository roomRepository, TransferRepository transferRepository)
    {
        _lock = new object();
        _lock2 = new object();
        _roomRepository = roomRepository;
        _inventoryRepository = inventoryRepository;
        _transferRepository = transferRepository;
        _inventoryRepository.OnRequestUpdate += (s, e) => UpdateTable(true);
        _transferRepository.OnRequestUpdate += (s, e) => UpdateTransfers();
        _allInventory = new ObservableCollection<InventoryViewModel>();
        _transfers = new ObservableCollection<TransferViewModel>();
        MoveSelectedInventoryItem = new DelegateCommand(o => MoveInventoryItem(), o => IsInventoryItemSelected());
        foreach (var inventoryItem in _inventoryRepository.GetNonDynamic())
        {
            _allInventory.Add(new InventoryViewModel(inventoryItem));
        }

        _inventory = _allInventory;
        UpdateTransfers();
        BindingOperations.EnableCollectionSynchronization(_inventory, _lock);
        BindingOperations.EnableCollectionSynchronization(_transfers, _lock2);
    }

    private void UpdateTransfers()
    {
        lock (_lock2)
        {
            _transfers = new ObservableCollection<TransferViewModel>();
            foreach (var transfer in _transferRepository.GetAll())
            {
                _transfers.Add(new TransferViewModel(transfer));
            }

            Transfers = _transfers;
        }
    }
    private void MoveInventoryItem()
    {
        if (SelectedInventoryItemVm != null)
        {
            var inventoryItemId = SelectedInventoryItemVm.Id;
            var roomId = SelectedInventoryItemVm.Room;
            var vm = new EquipmentTransferWindowViewModel(inventoryItemId, roomId, SelectedInventoryItemVm.Quantity, _roomRepository, _inventoryRepository, _transferRepository);
       
            var transferWindow = new EquipmentTransferWindowView()
                { DataContext = vm  };
            vm.OnRequestUpdate += (s, e) => UpdateTransfers();
            vm.OnRequestClose +=  (s, e) => transferWindow.Close();
            transferWindow.Show();
        }
    }

    private bool IsInventoryItemSelected()
    {
        return SelectedInventoryItemVm != null;
    }
}