using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Equipment;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Orders;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.Order;
using ZdravoCorp.Core.Repositories.Room;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.CronJobs;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class EquipmentTransferWindowViewModel : ViewModelBase

{
    
    public ICommand ConfirmTransfer { get; }
    public ICommand CancelTransfer { get; }

    private RoomRepository _roomRepository;
    private InventoryRepository _inventoryRepository;
    private ObservableCollection<RoomViewModel> _rooms;

    public IEnumerable<RoomViewModel> Rooms
    {
        get
        {
            return _rooms;
        }
        set
        {
            _rooms = new ObservableCollection<RoomViewModel>(value);
            OnPropertyChanged();
        }
    }
    public event EventHandler OnRequestClose;
    public int InventoryItemId { get; set; }
    

    public EquipmentTransferWindowViewModel(int inventoryItemId, RoomRepository roomRepository, InventoryRepository inventoryRepository)
    {
        _roomRepository = roomRepository;
        _inventoryRepository = inventoryRepository;
        InventoryItemId = inventoryItemId;
        InventoryItem inventoryItem = _inventoryRepository.GetInventoryById(inventoryItemId);
        ConfirmTransfer = new DelegateCommand(o => Confirm());
        CancelTransfer = new DelegateCommand(o => Cancel());
        foreach (var room in _roomRepository.GetAllExcept(inventoryItem.RoomId))
        {
            _rooms.Add(new RoomViewModel(room));
        }
    }

    private void Cancel()
    {
        OnRequestClose(this, new EventArgs());
    }

    private void Confirm()
    {
        OnRequestClose(this, new EventArgs());
    }
}