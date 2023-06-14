using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.Scheduling.Models;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.GUI.Main;
using ZdravoCorp.GUI.Scheduling.Views;

namespace ZdravoCorp.GUI.Scheduling.ViewModels;

public class AppointmentTableViewModel : ViewModelBase
{
    private readonly List<Appointment> _allAppointments;
    private ObservableCollection<AppointmentViewModel> _appointments;
    private readonly IScheduleService _scheduleService;
    private readonly IDoctorService _doctorService;
    private readonly Patient _patient;
    private readonly IRoomService _roomService;


    public AppointmentTableViewModel()
    {
    }

    public AppointmentTableViewModel(IScheduleService scheduleService,
        IDoctorService doctorService, Patient patient,IRoomService roomService)
    {
        _roomService = roomService;
        _patient = patient;
        _scheduleService = scheduleService;
        _appointments = new ObservableCollection<AppointmentViewModel>();
        _doctorService= doctorService;
        _allAppointments = _scheduleService.GetPatientAppointments(_patient.Email);
        UpdateTable(_allAppointments);
        NewAppointmentCommand = new DelegateCommand(o => NewAppointment());
        ChangeAppointmentCommand = new DelegateCommand(o => ChangeAppointmentComm());
        CancelAppointmentCommand = new DelegateCommand(o => CancelAppointmentComm());
        RecommendAppointmentCommand = new DelegateCommand(o => RecommendAppointmentComm());
    }

    //public ObservableCollection<AppointmentViewModel> Appointments => _appointments;
    public AppointmentViewModel SelectedAppointment { get; set; }

    public ICommand NewAppointmentCommand { get; set; }
    public ICommand ChangeAppointmentCommand { get; set; }
    public ICommand CancelAppointmentCommand { get; set; }
    public ICommand RecommendAppointmentCommand { get; set; }

    public ObservableCollection<AppointmentViewModel> Appointments
    {
        get => _appointments;
        set
        {
            _appointments = value;
            UpdateTable(_scheduleService.GetPatientAppointments(_patient.Email));
        }
    }

    private void UpdateTable(List<Appointment> appointments)
    {
        foreach (var appointment in appointments) _appointments.Add(new AppointmentViewModel(appointment));
    }


    private void ChangeAppointmentComm()
    {
        var selectedAppointment = SelectedAppointment;
        if (selectedAppointment != null)
        {
            var appointment = _scheduleService.GetAppointmentById(selectedAppointment.Id);
            var isOnTime = appointment.Time.GetTimeBeforeStart(DateTime.Now) > 24;
            if (isOnTime)
            {
                var window = new ChangeAppointmentView
                {
                    DataContext = new ChangeAppointmentViewModel(selectedAppointment,
                        _scheduleService, Appointments, _doctorService, _patient)
                };
                window.Show();
            }
            else
            {
                MessageBox.Show("You are too late", "Error", MessageBoxButton.OK);
            }
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }

    public void NewAppointment()
    {
        var window = new MakeAppointmentView
        {
            DataContext = new MakeAppointmentViewModel(_scheduleService, Appointments,
                _doctorService, _patient, "",_roomService)
        };
        //var window = new MakeAppointmentView(_doctorRepository, _scheduleService, Appointments, _patient);
        window.Show();
    }

    public void CancelAppointmentComm()
    {
        var selectedAppointment = SelectedAppointment;
        if (selectedAppointment != null)
        {
            var appointment = _scheduleService.GetAppointmentById(selectedAppointment.Id);
            var isOnTime = appointment.Time.GetTimeBeforeStart(DateTime.Now) > 24;
            if (isOnTime)
            {
                _scheduleService.CancelAppointment(appointment.Id);
                Appointments.Remove(GetById(selectedAppointment.Id, Appointments));
            }
            else
            {
                MessageBox.Show("You are too late", "Error", MessageBoxButton.OK);
            }
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }

    public void RecommendAppointmentComm()
    {
        var window = new AdvancedMakeAppointmentView
        {
            DataContext = new AdvancedMakeAppointmentViewModel(_doctorService, _scheduleService, _patient, Appointments)
        };
        window.Show();
    }

    public AppointmentViewModel GetById(int id, ObservableCollection<AppointmentViewModel> Appointments)
    {
        foreach (var appointmentViewModel in Appointments)
            if (appointmentViewModel.Id == id)
                return appointmentViewModel;

        return null;
    }
}