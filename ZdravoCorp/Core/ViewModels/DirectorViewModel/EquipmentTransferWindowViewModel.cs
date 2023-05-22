using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Transfers;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.RoomRepo;
using ZdravoCorp.Core.Repositories.TransfersRepo;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.RoomServices;
using ZdravoCorp.Core.Services.TransferServices;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.CronJobs;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class EquipmentTransferWindowViewModel : ViewModelBase

{
    private int _inputQuantity;
    private readonly InventoryItem _inventoryItem;
    private readonly IInventoryService _inventoryService;
    private readonly ITransferService _transferService;
    private readonly IRoomService _roomService;
    private readonly int _quantity;

    
    private ObservableCollection<RoomViewModel> _rooms;
    private readonly int _sourceRoomId;
    


    public EquipmentTransferWindowViewModel(int inventoryItemId, int roomId, int quantity,
        IRoomService roomService, IInventoryService inventoryService, ITransferService transferService)
    {
        _rooms = new ObservableCollection<RoomViewModel>();
        _roomService = roomService;
        _inventoryService = inventoryService;
        _transferService = transferService;
        InventoryItemId = inventoryItemId;
        _sourceRoomId = roomId;
        _quantity = quantity;
        Quantity = 0;
        MaxQuantity = "Quantity(max " + _quantity + "):";
        _inventoryItem = _inventoryService.GetById(inventoryItemId);
        ConfirmTransfer = new DelegateCommand(o => Confirm(), o => CanConfirm());
        CancelTransfer = new DelegateCommand(o => Cancel());
        foreach (var room in _roomService.GetAllExcept(_inventoryItem.RoomId)) _rooms.Add(new RoomViewModel(room));

        InitComboBoxes();
    }

    public ICommand ConfirmTransfer { get; }
    public ICommand CancelTransfer { get; }
    public RoomViewModel? SelectedRoom { get; set; }

    public int[]? Hour { get; set; }
    public int[]? Minute { get; set; }

    public int? SelectedHour { get; set; }
    public int? SelectedMinute { get; set; }
    public DateTime? SelectedDate { get; set; }
    public string MaxQuantity { get; }

    public int Quantity
    {
        get => _inputQuantity;
        set
        {
            _inputQuantity = value;
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public IEnumerable<RoomViewModel> Rooms
    {
        get => _rooms;
        set
        {
            _rooms = new ObservableCollection<RoomViewModel>(value);
            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public int InventoryItemId { get; set; }
    public event EventHandler? OnRequestClose;
    //public event EventHandler? OnRequestUpdate;

    private void InitComboBoxes()
    {
        Hour = new int[24];
        Minute = new int[60];
        for (var i = 0; i < 24; i++) Hour[i] = i;

        for (var i = 0; i < 60; i++) Minute[i] = i;
    }

    private bool CanConfirm()
    {
        return SelectedDate != null && SelectedHour != null && SelectedMinute != null && SelectedRoom != null &&
               Quantity != 0 && Quantity < _quantity;
        
        // TODO not valid need checking
    }

    private void Cancel()
    {
        OnRequestClose(this, new EventArgs());
    }

    private void Confirm()
    {
        var tempDate = (DateTime)SelectedDate;
        var when = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, (int)SelectedHour, (int)SelectedMinute, 0);

        var newTransfer = new TransferDTO(IDGenerator.GetId(), _roomService.GetById(_sourceRoomId),
            _roomService.GetById(SelectedRoom.Id), when, Quantity, InventoryItemId, _inventoryItem.Equipment.Name, Transfer.TransferStatus.Pending);
        _transferService.AddTransfer(newTransfer);
        JobScheduler.TransferRequestTaskScheduler(newTransfer);
        OnRequestClose?.Invoke(this, new EventArgs());
    }
}