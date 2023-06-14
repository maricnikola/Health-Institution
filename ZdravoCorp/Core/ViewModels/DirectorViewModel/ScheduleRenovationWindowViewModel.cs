using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Autofac;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Renovation;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Services.RenovationServices;
using ZdravoCorp.Core.Services.RoomServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.CronJobs;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class ScheduleRenovationWindowViewModel : ViewModelBase
{
    private IRenovationService _renovationService;
    private IRoomService _roomService;
    private IScheduleService _scheduleService;
    public event EventHandler? OnRequestClose = null;
    public ICommand ConfirmRenovation { get; }
    public ICommand CancelRenovation { get; }
    private TimeSlot? _renovationTimeSlot;
    public int SelectedRoom { get; }
    public string? SelectedRoomType { get; }
    public DateTime? SelectedStartDate { get; set; }
    public DateTime? SelectedEndDate { get; set; }
    public int SelectedStartHour { get; set; }
    public int SelectedEndHour { get; set; }
    public int SelectedStartMinute { get; set; }
    public int SelectedEndMinute { get; set; }
    private bool _isRoomAvailable;

    public bool IsRoomAvailable
    {
        get => _isRoomAvailable;
        set
        {
            _isRoomAvailable = value;
            OnPropertyChanged();
        }
    }

    private bool _isSplitEnabled;

    public bool IsSplitEnabled
    {
        get => _isSplitEnabled;
        set
        {
            _isSplitEnabled = value;
            OnPropertyChanged();
        }
    }

    private bool _isJoinEnabled;

    public bool IsJoinEnabled
    {
        get => _isJoinEnabled;
        set
        {
            _isJoinEnabled = value;
            OnPropertyChanged();
        }
    }

    private bool _isJoinChecked;
    private bool _isSplitChecked;

    public bool IsJoinChecked
    {
        get => _isJoinChecked;
        set
        {
            IsSplitEnabled = !value;
            _isJoinChecked = value;
            OnPropertyChanged();
        }
    }

    public bool IsSplitChecked
    {
        get => _isSplitChecked;
        set
        {
            IsJoinEnabled = !value;
            _isSplitChecked = value;
            OnPropertyChanged();
        }
    }

    private bool _isJoinRoomAvailable;

    public bool IsJoinRoomAvailable
    {
        get => _isJoinRoomAvailable;
        set
        {
            _isJoinRoomAvailable = value;
            OnPropertyChanged();
        }
    }

    public string SplitRoomType { get; set; } = "";
    public int JoinRoomId { get; set; } = 0;
    public int[]? Hours { get; set; }
    public int[]? Minutes { get; set; }
    public string[]? RoomTypes { get; set; }
    public List<int> JoinRoomList { get; set; }
    
    
    
    
    
    public ScheduleRenovationWindowViewModel(IRenovationService renovationService, IRoomService roomService, int roomId)
    {
        _renovationService = renovationService;
        _roomService = roomService;
        _scheduleService = Injector.Container.Resolve<IScheduleService>();
        ConfirmRenovation = new DelegateCommand(o => Confirm(), o => CanConfirm());
        CancelRenovation = new DelegateCommand(o => Cancel());
        SelectedRoom = roomId;
        SelectedRoomType = _roomService.GetById(roomId)?.Type.ToString();
        IsSplitEnabled = true;
        IsJoinEnabled = true;
        PopulateComboBoxes();
    }

    private void Cancel()
    {
        OnRequestClose?.Invoke(this, new EventArgs());
    }

    private void Confirm()
    {
        Room? split = null;
        Room? join = null;

        if (IsSplitChecked)
        {
            if (Enum.TryParse<RoomType>(SelectedRoomType, out var type))
                split = new Room(IDGenerator.GetId(), type,false);
        }

        if (IsJoinChecked)
        {
            join = (_roomService.GetById(JoinRoomId));
        }

        var newRenovation = new RenovationDTO(IDGenerator.GetId(), _roomService.GetById(SelectedRoom),
            _renovationTimeSlot, Renovation.RenovationStatus.Pending, split, join);
        _renovationService.AddRenovation(newRenovation);
        JobScheduler.RenovationTaskScheduler(newRenovation);
        OnRequestClose?.Invoke(this, EventArgs.Empty);
    }

    private bool CanConfirm()
    {
        if (SelectedStartDate != null && SelectedEndDate != null)
        {
            if (GenerateTimeSpan() && _scheduleService.CheckRoomAvailability(SelectedRoom, _renovationTimeSlot) && !_renovationService.IsRenovationScheduled(SelectedRoom, _renovationTimeSlot))
            {
                IsRoomAvailable = true;
                if (IsSplitChecked && SplitRoomType == "")
                {
                    return false;
                }
                if (IsJoinChecked)
                {
                    if (_scheduleService.HasAppointmentsAfter(SelectedRoom, _renovationTimeSlot.Start) ||
                        _renovationService.HasRenovationsAfter(SelectedRoom, _renovationTimeSlot.Start))
                    {
                        IsRoomAvailable = false;
                        return false;
                    }
                    if (JoinRoomId != 0 && !_roomService.GetById(JoinRoomId).IsUnderRenovation&&_scheduleService.CheckRoomAvailability(JoinRoomId, _renovationTimeSlot) && !_renovationService.IsRenovationScheduled(JoinRoomId, _renovationTimeSlot))
                    {
                        IsJoinRoomAvailable = true;
                        return true;
                    }

                    IsJoinRoomAvailable = false;
                    return false;
                }
                else
                {
                    IsJoinRoomAvailable = false;
                    return true;
                }
            }
        }
        IsRoomAvailable = false;
        return false;
    }

    private bool GenerateTimeSpan()
    {
        DateTime start = new DateTime(SelectedStartDate.Value.Year, SelectedStartDate.Value.Month,
            SelectedStartDate.Value.Day, SelectedStartHour, SelectedStartMinute, 0);
        DateTime end = new DateTime(SelectedEndDate.Value.Year, SelectedEndDate.Value.Month, SelectedEndDate.Value.Day,
            SelectedEndHour, SelectedEndMinute, 0);
        if (start >= end || start < DateTime.Now || end < DateTime.Now)
            return false;
        _renovationTimeSlot = new TimeSlot(start, end);
        return true;
    }

    private void PopulateComboBoxes()
    {
        Minutes = new[] { 00, 15, 30, 45 };
        Hours = new[]
            { 00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        RoomTypes = new[] {RoomType.OperationRoom.ToString(), RoomType.ExaminationRoom.ToString(), RoomType.PatientRoom.ToString(), RoomType.WaitingRoom.ToString()};
        JoinRoomList = new List<int>(_roomService.GetAllIds());
        JoinRoomList.Remove(999);
        JoinRoomList.Remove(SelectedRoom);

    }



}