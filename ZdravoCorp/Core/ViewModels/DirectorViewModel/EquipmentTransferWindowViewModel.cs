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

public class EquipmentTransferWindowViewModel : ViewModelBase

{
    
    public ICommand ConfirmTransfer { get; }
    public ICommand CancelTransfer { get; }

    private RoomRepository _roomRepository;
    private InventoryRepository _inventoryRepository;
    private TransferRepository _transferRepository;
    private ObservableCollection<RoomViewModel> _rooms;
    public RoomViewModel? SelectedRoom { get; set; }
    
    public int[] Hour { get; set; }
    public int[] Minute { get; set; }
    
    public int? SelectedHour { get; set; }
    public int? SelectedMinute { get; set; }
    public DateTime? SelectedDate { get; set; }

    private int _quantity;
    public string MaxQuantity { get; }
    
    public int Quantity { get; set; }
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
    public event EventHandler OnRequestUpdate;
    public int InventoryItemId { get; set; }
    private int _sourceRoomId;
    private InventoryItem _inventoryItem;
    

    public EquipmentTransferWindowViewModel(int inventoryItemId, int roomId, int quantity, RoomRepository roomRepository, InventoryRepository inventoryRepository, TransferRepository transferRepository)
    {
        _rooms = new ObservableCollection<RoomViewModel>();
        _roomRepository = roomRepository;
        _inventoryRepository = inventoryRepository;
        _transferRepository = transferRepository;
        InventoryItemId = inventoryItemId;
        _sourceRoomId = roomId;
        _quantity = quantity;
        Quantity = 0;
        MaxQuantity = $"Quantity(max "+_quantity.ToString()+"):";
        _inventoryItem = _inventoryRepository.GetInventoryById(inventoryItemId);
        ConfirmTransfer = new DelegateCommand(o => Confirm(), o => CanConfirm());
        CancelTransfer = new DelegateCommand(o => Cancel());
        foreach (var room in _roomRepository.GetAllExcept(_inventoryItem.RoomId))
        {
            _rooms.Add(new RoomViewModel(room));
        }

        InitComboBoxes();
    }

    private void InitComboBoxes()
    {
        Hour = new int[24];
        Minute = new int[60];
        for (int i = 0; i < 24; i++)
        {
            Hour[i] = i;
        }

        for (int i = 0; i < 60; i++)
        {
            Minute[i] = i;
        }
    }

    private bool CanConfirm()
    {
        return SelectedDate != null && SelectedHour != null && SelectedMinute != null && SelectedRoom != null && Quantity != 0 && Quantity < _quantity;
    }

    private void Cancel()
    {
        OnRequestClose(this, new EventArgs());
    }

    private void Confirm()
    {
        DateTime tempDate = (DateTime)SelectedDate;
        DateTime when = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, (int)SelectedHour, (int)SelectedMinute, 0);

        Transfer newTransfer = new Transfer(IDGenerator.GetId(),_roomRepository.GetById(_sourceRoomId),
            _roomRepository.GetById(SelectedRoom.Id), when, Quantity, InventoryItemId,_inventoryItem.Equipment.Name);
        _transferRepository.Add(newTransfer);
        Serializer.Save(_transferRepository);
        JobScheduler.TransferRequestTaskScheduler(newTransfer);
        //_inventoryRepository.GetInventoryById(InventoryItemId).UpdateRoom(_roomRepository.GetById(SelectedRoom.Id));
        OnRequestUpdate(this, new EventArgs());
        OnRequestClose(this, new EventArgs());
    }
}