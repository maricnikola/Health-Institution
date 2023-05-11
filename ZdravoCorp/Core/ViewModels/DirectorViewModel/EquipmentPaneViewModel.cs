using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using ZdravoCorp.Core.Models.Equipment;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Repositories.Inventory;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class EquipmentPaneViewModel : ViewModelBase
{
    private InventoryRepository _inventoryRepository;
    private ObservableCollection<InventoryViewModel> _inventory;
    private ObservableCollection<InventoryViewModel> _filteredInventory;
    private ObservableCollection<InventoryViewModel> _allInventory;
    private ObservableCollection<string> _roomTypes;
    private ObservableCollection<string> _equipmentTypes;
    private ObservableCollection<string> _quantities;
    private object _lock;
    private string _searchText = "";
    private string _selectedRoomType = "None";
    private string _selectedEquipmentType = "None";
    private string _selectedQuantity = "None";
    private bool _warehouseChecked = false;
    
    public bool IsWarehouseChecked
    {
        get { return _warehouseChecked; }
        set
        {
            _warehouseChecked = value;
            UpdateTable();
            OnPropertyChanged("IsWarehouseChecked");
        }
    }

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

    public string SelectedRoomType
    {
        get { return _selectedRoomType; }
        set
        {
            _selectedRoomType = value;
            UpdateTable();
            OnPropertyChanged("SelectedRoomType");
        }
    }

    public string SelectedEquipmentType
    {
        get { return _selectedEquipmentType; }
        set
        {
            _selectedEquipmentType = value;
            UpdateTable();
            OnPropertyChanged("SelectedEquipmentType");
        }
    }

    public string SelectedQuantity
    {
        get { return _selectedQuantity; }
        set
        {
            _selectedQuantity = value;
            UpdateTable();
            OnPropertyChanged("SelectedQuantity");
        }
    }

    private void UpdateTable()
    {
        lock (_lock)
        {
            _filteredInventory = _allInventory;
            var wh = _filteredInventory.Intersect(UpdateTableFromWarehouseChecked());
            var f1 = wh.Intersect(UpdateTableFromSearch());
            var f2 = f1.Intersect(UpdateTableFromEquipmentType());
            var f3 = f2.Intersect(UpdateTableFromRoomType());
            var f4 = f3.Intersect(UpdateTableFromQuantity());

            Inventory = f4;
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

    private IEnumerable<InventoryViewModel> UpdateTableFromRoomType()
    {
        if (_selectedRoomType != "None")
        {
            return new ObservableCollection<InventoryViewModel>(FilterByRoomType(_selectedRoomType));
        }
        else
        {
            return _allInventory;
        }
    }

    private ObservableCollection<InventoryViewModel> UpdateTableFromEquipmentType()
    {
        if (_selectedEquipmentType != "None")
        {
            return new ObservableCollection<InventoryViewModel>(FilterRoomByEquipmentType(_selectedEquipmentType));
        }
        else
        {
            return _allInventory;
        }
    }

    private ObservableCollection<InventoryViewModel> UpdateTableFromQuantity()
    {
        if (_selectedQuantity != "None")
        {
            return new ObservableCollection<InventoryViewModel>(FilterByQuantity(_selectedQuantity));
        }
        else
        {
            return _allInventory;
        }
    }

    private ObservableCollection<InventoryViewModel> UpdateTableFromWarehouseChecked()
    {
        if (_warehouseChecked == false)
        {
            return new ObservableCollection<InventoryViewModel>(FilterFromWarehouse());
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

    public IEnumerable<InventoryViewModel> FilterFromWarehouse()
    {
        return _allInventory.Where(item => item.RoomType != RoomType.StockRoom.ToString());
    }

    public IEnumerable<InventoryViewModel> FilterByRoomType(string roomType)
    {
        return _allInventory.Where(item => item.RoomType == roomType);
    }

    public IEnumerable<InventoryViewModel> FilterRoomByEquipmentType(string equipmentType)
    {
        return _allInventory.Where(item => item.Type == equipmentType);
    }

    public IEnumerable<InventoryViewModel> FilterByQuantity(string quantity)
    {
        if (quantity == "not in stock")
        {
            return _allInventory.Where(item => item.Quantity == 0);
        }

        if (quantity == "0-10")
        {
            return _allInventory.Where(item => item.Quantity < 10);
        }

        if (quantity == "10+")
        {
            return _allInventory.Where(item => item.Quantity >= 10);
        }

        return null;
    }

    public ObservableCollection<string> Quantities
    {
        get { return _quantities; }
    }

    public ObservableCollection<string> RoomTypes
    {
        get { return _roomTypes; }
    }

    public ObservableCollection<string> EquipmentTypes
    {
        get { return _equipmentTypes; }
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

    public EquipmentPaneViewModel(InventoryRepository inventoryRepository)
    {
        _lock = new object();
        _inventoryRepository = inventoryRepository;
        _inventoryRepository.OnRequestUpdate += (s, e) => UpdateTable();
        _allInventory = new ObservableCollection<InventoryViewModel>();
        foreach (var inventoryItem in _inventoryRepository.GetAll())
        {
            _allInventory.Add(new InventoryViewModel(inventoryItem));
        }
        _inventory = _allInventory;
        _equipmentTypes = new ObservableCollection<string>();
        _roomTypes = new ObservableCollection<string>();
        _quantities = new ObservableCollection<string>();
        BindingOperations.EnableCollectionSynchronization(_inventory, _lock);
        LoadComboBoxCollections();
    }


    public void LoadComboBoxCollections()
    {
        _roomTypes.Add("None");
        _roomTypes.Add(RoomType.OperationRoom.ToString());
        _roomTypes.Add(RoomType.WaitingRoom.ToString());
        _roomTypes.Add(RoomType.StockRoom.ToString());
        _roomTypes.Add(RoomType.PatientRoom.ToString());
        _roomTypes.Add(RoomType.ExaminationRoom.ToString());

        _equipmentTypes.Add("None");
        _equipmentTypes.Add(Equipment.EquipmentType.Room.ToString());
        _equipmentTypes.Add(Equipment.EquipmentType.Examination.ToString());
        _equipmentTypes.Add(Equipment.EquipmentType.Hallway.ToString());
        _equipmentTypes.Add(Equipment.EquipmentType.Operation.ToString());

        _quantities.Add("None");
        _quantities.Add("not in stock");
        _quantities.Add("0-10");
        _quantities.Add("10+");
    }
}