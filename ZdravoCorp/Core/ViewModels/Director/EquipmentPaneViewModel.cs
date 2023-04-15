using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ZdravoCorp.Core.Models.Equipment;
using ZdravoCorp.Core.Models.Room;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.View;

namespace ZdravoCorp.Core.ViewModels;

public class EquipmentPaneViewModel : ViewModelBase
{
    private InventoryRepository _inventoryRepository;
    private ObservableCollection<InventoryViewModel> _inventory;
    private ObservableCollection<InventoryViewModel> _filteredInventory;
    private ObservableCollection<InventoryViewModel> _allInventory;
    private ObservableCollection<string> _roomTypes;
    private ObservableCollection<string> _equipmentTypes;
    private ObservableCollection<string> _quantities;

    private string _searchText = "";
    private string _selectedRoomType = "None";
    private string _selectedEquipmentType = "None";
    private string _selectedQuantity = "None";

    public EquipmentPaneViewModel()
    {
        
    }
    public string SearchBox
    {
        get { return _searchText; }
        set
        {
            _searchText = value;
            UpdateTableFromSearch();
            OnPropertyChanged("Search");
        }
    }

    public string SelectedRoomType
    {
        get { return _selectedRoomType; }
        set
        {
            _selectedRoomType = value;
            UpdateTableFromRoomType();
            OnPropertyChanged("SelectedRoomType");
        }
    }

    public string SelectedEquipmentType
    {
        get { return _selectedEquipmentType; }
        set
        {
            _selectedEquipmentType = value;
            UpdateTableFromEquipmentType();
            OnPropertyChanged("SelectedEquipmentType");
        }
    }
    
    public string SelectedQuantity
    {
        get { return _selectedQuantity; }
        set
        {
            _selectedQuantity = value;
            UpdateTableFromQuantity();
            OnPropertyChanged("SelectedQuantity");
        }
    }

    private void UpdateTableFromSearch()
    {
        if (_searchText != "")
        {
           _filteredInventory= new ObservableCollection<InventoryViewModel>(_inventory.Intersect(Search(_searchText)));
            Inventory = _filteredInventory;
        }
        else
        {
            
            Inventory = _allInventory;
        }
    }    
    private void UpdateTableFromRoomType()
    {
        if (_selectedRoomType != "None")
        {
           _filteredInventory= new ObservableCollection<InventoryViewModel>(_inventory.Intersect(FilterByRoomType(_selectedRoomType)));
            Inventory = _filteredInventory;
        }
        else
        {
            Inventory = _allInventory;
        }
    }   
    private void UpdateTableFromEquipmentType()
    {
        if (_selectedEquipmentType != "None")
        {
           _filteredInventory= new ObservableCollection<InventoryViewModel>(_inventory.Intersect(FilterRoomByEquipmentType(_selectedEquipmentType)));
            Inventory = _filteredInventory;
        }
        else
        {
            Inventory = _allInventory;
        }
    }   
    private void UpdateTableFromQuantity()
    {
        if (_selectedQuantity != "None")
        {
           _filteredInventory= new ObservableCollection<InventoryViewModel>(_inventory.Intersect(FilterByQuantity(_selectedQuantity)));
            Inventory = _filteredInventory;
        }
        else
        {
            Inventory = _allInventory;
        }
    }
    
    
    
    public IEnumerable<InventoryViewModel> Search(string inputText)
    {
        return _allInventory.Where(item => item.ToString().Contains(inputText));
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
        get
        {
            return _inventory;
            
        }
        set
        {
           
            _inventory =new ObservableCollection<InventoryViewModel>(value);
            OnPropertyChanged();
        }
    }

    public EquipmentPaneViewModel(InventoryRepository inventoryRepository )
    {
        _inventoryRepository = inventoryRepository;
        _allInventory = new ObservableCollection<InventoryViewModel>();
        foreach (var inventoryItem in _inventoryRepository.GetAll())
        {
            _allInventory.Add(new InventoryViewModel(inventoryItem));
        }

        _inventory = _allInventory;

        _equipmentTypes = new ObservableCollection<string>();
        _roomTypes = new ObservableCollection<string>();
        _quantities = new ObservableCollection<string>();
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