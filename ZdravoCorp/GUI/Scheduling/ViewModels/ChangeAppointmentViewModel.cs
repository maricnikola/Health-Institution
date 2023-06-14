using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.Scheduling.ViewModels;

public class ChangeAppointmentViewModel : ViewModelBase
{
    private readonly ObservableCollection<string> _doctors;
    private readonly AppointmentViewModel _appointmentViewModel;
    private DateTime _date = DateTime.Now + TimeSpan.FromHours(1);

    private string _selectedDoctor;
    private readonly IDoctorService _doctorService;
    private int _hours;
    private int _minutes;
    private readonly Patient _patient;
    private readonly IScheduleService _scheduleService;

    public ChangeAppointmentViewModel(AppointmentViewModel appointmentViewModel, IScheduleService scheduleService,
        ObservableCollection<AppointmentViewModel> Appointments, IDoctorService doctorService, Patient patient)
    {
        _doctorService= doctorService;
        _scheduleService = scheduleService;
        _patient = patient;
        _appointmentViewModel = appointmentViewModel;
        _doctors = new ObservableCollection<string>();
        PossibleMinutes = new[] { 00, 15, 30, 45 };
        PossibleHours = new[]
            { 00, 01, 02, 03, 04, 05, 06, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        var doctors = _doctorService.GetAll();
        foreach (var doctor in doctors) _doctors.Add(doctor.FullName + "-" + doctor.Email);

        _selectedDoctor = _appointmentViewModel.DoctorName+ "-" + _appointmentViewModel.DoctorEmail;
        _date = _appointmentViewModel.Date;
        _hours = _appointmentViewModel.Date.Hour;
        _minutes = _appointmentViewModel.Date.Minute;

        ChangeAppointmentCommand = new DelegateCommand(o => ChangeAppointmentComm(Appointments));
    }

    public IEnumerable<string> AllDoctors => _doctors;
    public int[] PossibleMinutes { get; set; }
    public int[] PossibleHours { get; set; }

    public ICommand ChangeAppointmentCommand { get; set; }

    public string SelectedDoctor
    {
        get => _selectedDoctor;
        set
        {
            _selectedDoctor = value;
            OnPropertyChanged();
        }
    }

    public DateTime Date
    {
        get => _date;
        set
        {
            _date = value;
            OnPropertyChanged();
        }
    }

    public int Hours
    {
        get => _hours;
        set
        {
            _hours = value;
            OnPropertyChanged();
        }
    }

    public int Minutes
    {
        get => _minutes;
        set
        {
            _minutes = value;
            OnPropertyChanged();
        }
    }

    private void ChangeAppointmentComm(ObservableCollection<AppointmentViewModel> Appointments)
    {
        try
        {
            var h = Hours;
            var m = Minutes;
            var d = Date;
            var dm = SelectedDoctor;

            var start = new DateTime(d.Year, d.Month, d.Day, h, m, 0);
            var end = start.AddMinutes(15);
            var time = new TimeSlot(start, end);

            var tokens = dm.Split("-");
            var mail = tokens[1];
            var doctor = _doctorService.GetByEmail(mail);

            var appointment =
                _scheduleService.ChangeAppointment(_appointmentViewModel.Id, time, doctor, _patient.Email);
            if (appointment != null)
            {
                Appointments.Remove(GetById(appointment.Id, Appointments));
                Appointments.Add(new AppointmentViewModel(appointment));
                MessageBox.Show("Appointment changed seccessfully", "Success", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Invalid Appointment", "Error", MessageBoxButton.OK);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("Invalid Appointment", "Error", MessageBoxButton.OK);
        }
    }

    private AppointmentViewModel GetById(int id, ObservableCollection<AppointmentViewModel> Appointments)
    {
        return Appointments.FirstOrDefault(appointmentViewModel => appointmentViewModel.Id == id);
    }
}