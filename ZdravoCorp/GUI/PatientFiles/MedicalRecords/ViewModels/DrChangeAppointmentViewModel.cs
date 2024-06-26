﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Repositories;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.PatientFiles.MedicalRecords.Models;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.GUI.Main;
using ZdravoCorp.GUI.Scheduling.ViewModels;

namespace ZdravoCorp.GUI.PatientFiles.MedicalRecords.ViewModels;

internal class DrChangeAppointmentViewModel : ViewModelBase
{
    private readonly AppointmentViewModel _appointmentModel;
    private readonly DateTime _date;
    private readonly Doctor _dr;
    private readonly Patient _patient;
    private IPatientService _patientService;
    private readonly IScheduleService _scheduleService;


    private DateTime _startDateChange;

    private int _startTimeHours;
    private int _startTimeMinutes;


    public DrChangeAppointmentViewModel(AppointmentViewModel appointmentModel, IScheduleService scheduleService,
        IDoctorService doctorService, ObservableCollection<AppointmentViewModel> appointment,
        IPatientService patientService, Doctor doctor, Patient patient, AppointmentViewModel appointmentSelected,
        DateTime date)
    {
        _dr = doctor;
        _appointmentModel = appointmentModel;
        _patient = patient;
        _appointmentModel = appointmentSelected;
        _scheduleService = scheduleService;
        _patientService = patientService;
        _date = date;
        var _controller = new PatientRepository();
        var patients = _controller.Patients;

        _patientsFullname = new ObservableCollection<string>();
        foreach (var p in patients) _patientsFullname.Add(p.FullName + "-" + p.Email);

        _startDateChange = appointmentModel.Date;
        _startTimeHours = _appointmentModel.Date.Hour;
        _startTimeMinutes = _appointmentModel.Date.Minute;
        PossibleMinutes = new[] { 00, 15, 30, 45 };
        PossibleHours = new[]
            { 00, 01, 02, 03, 04, 05, 06, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        ChangeCommand = new DelegateCommand(o => DrChangeAppointment(appointment));
        CancelCommand = new DelegateCommand(o => CloseWindow());
    }

    private ObservableCollection<string> _patientsFullname { get; }
    public IEnumerable<string> Patients => _patientsFullname;
    public int[] PossibleMinutes { get; set; }
    public int[] PossibleHours { get; set; }

    public DateTime StartDateChange
    {
        get => _startDateChange;
        set
        {
            _startDateChange = value;
            OnPropertyChanged();
        }
    }

    public int StartTimeHours
    {
        get => _startTimeHours;
        set
        {
            _startTimeHours = value;
            OnPropertyChanged();
        }
    }

    public int StartTimeMinutes
    {
        get => _startTimeMinutes;
        set
        {
            _startTimeMinutes = value;
            OnPropertyChanged();
        }
    }


    public ICommand ChangeCommand { get; }
    public ICommand CancelCommand { get; }

    private void CloseWindow()
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }

    public void DrChangeAppointment(ObservableCollection<AppointmentViewModel> Appointments)
    {
        try
        {
            var hours = StartTimeHours;
            var minutes = StartTimeMinutes;
            var d = StartDateChange;

            var start = new DateTime(d.Year, d.Month, d.Day, hours, minutes, 0);
            var end = start.AddMinutes(15);
            var time = new TimeSlot(start, end);

            var patientName = _patient.FirstName;


            var medicalRecord = new MedicalRecord(_patient);


            var appointment = _scheduleService.ChangeAppointment(_appointmentModel.Id, time, _dr, _patient.Email);


            if (appointment != null)
            {
                CloseWindow();
                if (_scheduleService.IsForShow(appointment, _date))
                {
                    Appointments.Remove(_appointmentModel);
                    Appointments.Add(new AppointmentViewModel(appointment));
                }
                else
                {
                    Appointments.Remove(_appointmentModel);
                }
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
}