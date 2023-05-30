using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Services.InventoryServices;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DynamicEquipmentTransferWindowViewModel : ViewModelBase, IDataErrorInfo

{
    private readonly IInventoryService _inventoryService;
    private int _maxMoveQuantity;
    private int _moveQuantity;
    private string _moveQuantityString;
    private ObservableCollection<SourceRoomViewModel> _rooms;


    public DynamicEquipmentTransferWindowViewModel(int inventoryItemId, int roomId, int quantity,
        IInventoryService inventoryService)
    {
        _rooms = new ObservableCollection<SourceRoomViewModel>();
        _inventoryService = inventoryService;
        InventoryItemId = inventoryItemId;
        DestinationRoom = roomId;
        Quantity = quantity;
        MoveQuantity = 0;
        MaxMoveQuantity = -1;
        var inventoryItem = _inventoryService.GetById(inventoryItemId);
        RoomType = inventoryItem?.Room.Type.ToString();
        MItem = inventoryItem.Equipment.Name;

        foreach (var item in _inventoryService.LocateItemInOtherRooms(inventoryItem)) _rooms.Add(new SourceRoomViewModel(item));
        ConfirmTransfer = new DelegateCommand(o => Confirm(), o => CanConfirm());
        CancelTransfer = new DelegateCommand(o => Cancel());
    }

    public ICommand ConfirmTransfer { get; set; }

    public ICommand CancelTransfer { get; }
    public SourceRoomViewModel? SelectedRoom { get; set; }
    public int Quantity { get; set; }
    public int InventoryItemId { get; set; }

    public string MoveQuantityString
    {
        get => _moveQuantityString;
        set
        {
            if (SelectedRoom != null)
                _maxMoveQuantity = SelectedRoom.Quantity;
            else
                _maxMoveQuantity = -1;

            _moveQuantityString = value;
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public int MoveQuantity
    {
        get
        {
            if (int.TryParse(MoveQuantityString, out int value))
                return value;
            return 0;
        }
        set => _moveQuantity = value;
    }


    public int MaxMoveQuantity { get; set; }
    public int DestinationRoom { get; set; }
    public string RoomType { get; set; }
    public string? MItem { get; set; }

    public IEnumerable<SourceRoomViewModel> Rooms
    {
        get => _rooms;
        set
        {
            _rooms = new ObservableCollection<SourceRoomViewModel>(value);
            CommandManager.InvalidateRequerySuggested();
            OnPropertyChanged();
        }
    }

    public event EventHandler OnRequestClose;


    private bool CanConfirm()
    {
        return SelectedRoom != null && MoveQuantity > 0 && MoveQuantity <= _maxMoveQuantity;
    }

    private void Cancel()
    {
        OnRequestClose(this, new EventArgs());
    }

    private void Confirm()

    {
        _inventoryService.UpdateDestinationInventoryItem(SelectedRoom.ItemId, InventoryItemId, _moveQuantity);
        OnRequestClose(this, new EventArgs());
    }

    public string Error { get; }

    public string this[string columnName]
    {
        get
        {
            if (columnName == "MoveQuantityString")
            {
                if (string.IsNullOrEmpty(MoveQuantityString) || !int.TryParse(MoveQuantityString, out int value))
                {
                    return "error";
                }
            }

            return null;
        }
    }
}