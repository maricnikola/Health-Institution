using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.RenovationServices;
using ZdravoCorp.Core.Services.RoomServices;
using ZdravoCorp.Core.Services.TransferServices;
using ZdravoCorp.View.DirectorView;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class RenovationsViewModel : ViewModelBase
{
    private  ObservableCollection<RoomViewModel> _allRooms;
    private ObservableCollection<RoomViewModel> _rooms;
    private readonly IRoomService _roomService;
    private readonly object _lock;
    private readonly object _lock2;
    private string _searchText = "";
    private RoomViewModel? _selectedRoom;
    private readonly IRenovationService _renovationService;
    private ObservableCollection<RenovationViewModel> _renovations;

    public RenovationsViewModel (IRoomService roomService, IRenovationService renovationService)
    {
        _lock = new object();
        _lock2 = new object();
        _roomService = roomService;
        _renovationService = renovationService;
        _roomService.DataChanged += (s, e) => UpdateTable(true);
        _renovationService.DataChanged += (s, e) => UpdateRenovations();
        _allRooms = new ObservableCollection<RoomViewModel>();
        _renovations = new ObservableCollection<RenovationViewModel>();
        foreach (var room in _roomService.GetAllExcept(999))
            _allRooms.Add(new RoomViewModel(room));

        _rooms = _allRooms;
        UpdateRenovations();
        BindingOperations.EnableCollectionSynchronization(_rooms, _lock);
        BindingOperations.EnableCollectionSynchronization(_renovations, _lock2);
    }

    public RoomViewModel? SelectedRoom
    {
        get => _selectedRoom;
        set
        {
            _selectedRoom = value;
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public IEnumerable<RenovationViewModel> Renovations
    {
        get => _renovations;
        set
        {
            _renovations = new ObservableCollection<RenovationViewModel>(value);
            OnPropertyChanged();
        }
    }
    


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


    public IEnumerable<RoomViewModel> Rooms
    {
        get => _rooms;
        set
        {
            _rooms = new ObservableCollection<RoomViewModel>(value);
            OnPropertyChanged();
        }
    }


    private void UpdateTable(bool newAdded = false)
    {
        lock (_lock)
        {
            if (newAdded)
            {
                _allRooms = new ObservableCollection<RoomViewModel>();
                foreach (var room in _roomService.GetAllExcept(999))
                    _allRooms.Add(new RoomViewModel(room));
            }

            Rooms = UpdateTableFromSearch();
        }
    }

    private ObservableCollection<RoomViewModel> UpdateTableFromSearch()
    {
        if (_searchText != "")
            return new ObservableCollection<RoomViewModel>(Search(_searchText));
        return _allRooms;
    }

    public IEnumerable<RoomViewModel> Search(string inputText)
    {
        return _allRooms.Where(item => item.ToString().Contains(inputText));
    }

    private void UpdateRenovations()
    {
        lock (_lock2)
        {
            _renovations = new ObservableCollection<RenovationViewModel>();
            foreach (var renovation in _renovationService.GetAll()) _renovations.Add(new RenovationViewModel(renovation));

            Renovations = _renovations;
        }
    }
    

    private bool IsRoomSelected()
    {
        return SelectedRoom != null;
    }
}