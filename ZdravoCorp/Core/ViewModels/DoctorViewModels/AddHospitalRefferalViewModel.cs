using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.HospitalRefferals;
using ZdravoCorp.Core.Models.Therapies;
using ZdravoCorp.Core.Services.HospitalRefferalServices;
using ZdravoCorp.Core.Services.RoomServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.View.DoctorView;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class AddHospitalRefferalViewModel: ViewModelBase
{

    private PerformAppointmentViewModel _performAppointmentViewModel;

    public ICommand Close { get; }
    public ICommand CreateRefferal { get; }
    private IHospitalRefferalService _hospitalRefferalService;
    private IScheduleService _scheduleService;
    private IRoomService _roomService;
    private Appointment _appointment;
    private int _roomId;
    public AddHospitalRefferalViewModel(PerformAppointmentViewModel performAppointmentViewModel,Appointment appointment,
        IScheduleService scheduleService,IHospitalRefferalService hospitalRefferalService,IRoomService roomService)
    {
        _roomService = roomService;
        _appointment = appointment;
        _scheduleService = scheduleService;
        _hospitalRefferalService = hospitalRefferalService;
        _performAppointmentViewModel = performAppointmentViewModel;
        AssignRoom();
        Close = new DelegateCommand(o => CloseWindow(true));
        CreateRefferal = new DelegateCommand(o => CreateHospitalRefferal());
    }

    private int _duration;
    public int Duration
    {
        get
        {
            return _duration;
        }
        set
        {
            _duration = value;
            OnPropertyChanged(nameof(Duration));
        }
    }
    private string _initialTherapy;
    public string InitialTherapy
    {
        get
        {
            return _initialTherapy;
        }
        set
        {
            _initialTherapy = value;
            OnPropertyChanged(nameof(InitialTherapy));
        }
    }
    private string _additionTests;
    public string AdditionTests
    {
        get
        {
            return _additionTests;
        }
        set
        {
            _additionTests = value;
            OnPropertyChanged(nameof(AdditionTests));
        }
    }
    private void CloseWindow(bool backToPerform)
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
        if (backToPerform)
        {
            var performWindow = new PerformAppointmentView() { DataContext = _performAppointmentViewModel };
            performWindow.Show();
        }
    }
    private void AssignRoom()
    {
        foreach (var room in _roomService.GetAll())
        {
            if (room.IsUnderRenovation || room.Type != Models.Rooms.RoomType.PatientRoom 
                || !_scheduleService.CheckRoomAvailability(room.Id, _appointment.Time)) continue;
            _roomId = room.Id;
            return;
        }
    }
    private void CreateHospitalRefferal()
    {
        try
        {
            int duration = Duration;
            if(duration < 1)
            { 
                MessageBox.Show("Invalid data for hospital refferal", "Error", MessageBoxButton.OK);
                return;
            }
            TimeSlot time = new TimeSlot(_appointment.Time.End, _appointment.Time.End.AddDays(duration));
            string  therapyDescription = InitialTherapy;
            if(therapyDescription.Length < 5)
            {
                MessageBox.Show("Invalid data for hospital refferal", "Error", MessageBoxButton.OK);
                return;
            }
            string additionTests = AdditionTests;
            List<Therapy> initialTherapy = new List<Therapy>();
            Therapy therapy = new Therapy(therapyDescription, true);
            initialTherapy.Add(therapy);
            _hospitalRefferalService.AddRefferal(new HospitalRefferal(IDGenerator.GetId(),_appointment.PatientEmail,time, initialTherapy, additionTests,_roomId));
            CloseWindow(false);
            _performAppointmentViewModel.ShowDEquipmentSpentDialog();
            var appointmentDto = new AppointmentDTO(_appointment.Id, _appointment.Time.Start, _appointment.Doctor,
            _appointment.PatientEmail, null);
            _scheduleService.CancelAppointmentByDoctor(appointmentDto);
        }
        catch (Exception)
        {

            MessageBox.Show("Invalid data for hospital refferal", "Error", MessageBoxButton.OK);
        }
    }
}
