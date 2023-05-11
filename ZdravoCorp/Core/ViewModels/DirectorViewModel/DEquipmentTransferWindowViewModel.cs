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
using ZdravoCorp.Core.Models.Transfers;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.Order;
using ZdravoCorp.Core.Repositories.Room;
using ZdravoCorp.Core.Repositories.Transfers;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.CronJobs;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DEquipmentTransferWindowViewModel : ViewModelBase

{
    public ICommand ConfirmTransfer
    {
        get;
        set;
    }


    public ICommand CancelTransfer { get; }

    private RoomRepository _roomRepository;
    private InventoryRepository _inventoryRepository;
    private ObservableCollection<SourceRoomViewModel> _rooms;
    public SourceRoomViewModel? SelectedRoom { get; set; }

    private int _quantity;
    private int _moveQuantity;
    private int _maxMoveQuantity;

    public int Quantity { get; set; }

    public int MoveQuantity
    {
        get { return _moveQuantity; }
        set
        {
            if (SelectedRoom != null)
            {
                _maxMoveQuantity = SelectedRoom.Quantity;
            }
            else
            {
                _maxMoveQuantity = -1;
            }

            _moveQuantity = value;
            //_confirmTransfer.RaiseCanExecuteChanged();
        }
    }


    public int MaxMoveQuantity { get; set; }
    public int DestinationRoom { get; set; }
    public string RoomType { get; set; }
    public string? Item { get; set; }

    public IEnumerable<SourceRoomViewModel> Rooms
    {
        get { return _rooms; }
        set
        {
            _rooms = new ObservableCollection<SourceRoomViewModel>(value);
            //_confirmTransfer.RaiseCanExecuteChanged();
            OnPropertyChanged();
        }
    }

    public event EventHandler OnRequestClose;
    public int InventoryItemId { get; set; }
    private int _sourceRoomId;
    private InventoryItem _inventoryItem;


    public DEquipmentTransferWindowViewModel(int inventoryItemId, int roomId, int quantity,
        RoomRepository roomRepository, InventoryRepository inventoryRepository)
    {
        _rooms = new ObservableCollection<SourceRoomViewModel>();
        _roomRepository = roomRepository;
        _inventoryRepository = inventoryRepository;
        InventoryItemId = inventoryItemId;
        DestinationRoom = roomId;
        Quantity = quantity;
        MoveQuantity = 0;
        MaxMoveQuantity = -1;
        _inventoryItem = _inventoryRepository.GetInventoryById(inventoryItemId);
        RoomType = _inventoryItem.Room.Type.ToString();
        Item = _inventoryItem.Equipment.Name;
        ConfirmTransfer = new DelegateCommand(o => Confirm(), o => CanConfirm());

        CancelTransfer = new DelegateCommand(o => Cancel());
        foreach (var item in _inventoryRepository.LocateItem(_inventoryItem))
        {
            _rooms.Add(new SourceRoomViewModel(item));
        }
    }


    private bool CanConfirm()
    {
        return SelectedRoom != null && MoveQuantity <= _maxMoveQuantity;
    }

    private void Cancel()
    {
        OnRequestClose(this, new EventArgs());
    }

    private void Confirm()
    
    {
        _inventoryRepository.UpdateDestinationInventoryItem(SelectedRoom.ItemId, InventoryItemId, _moveQuantity);
        OnRequestClose(this, new EventArgs());
        _inventoryRepository.OnRequestUpdate.Invoke(this, new EventArgs());
    }
}