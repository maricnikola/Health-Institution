﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.MedicalRecord;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.TimeSlots;
using ZdravoCorp.View;

namespace ZdravoCorp.Core.ViewModels;

public class AppointmentTableViewModel: ViewModelBase
{
    private readonly ObservableCollection<AppointmentViewModel> _appointments;

    public ObservableCollection<AppointmentViewModel> Appointments => _appointments;
    public AppointmentViewModel SelectedAppointment { get; set; }
    private ScheduleRepository _controller;
    private DoctorRepository _doctorRepository;
    private Patient _patient;

    public ICommand NewAppointmentCommand { get; set; }
    public ICommand ChangeAppointmentCommand { get; set; }

    public AppointmentTableViewModel(List<Appointment> appointments, ScheduleRepository scheduleRepository, DoctorRepository doctorRepository, Patient patient)
    {
        _patient = patient;
        _controller = scheduleRepository;
        _appointments = new ObservableCollection<AppointmentViewModel>();
        _doctorRepository = doctorRepository;
        foreach (var appointment in appointments)
        {
            _appointments.Add(new AppointmentViewModel(appointment));
        }
        NewAppointmentCommand = new DelegateCommand(o => NewAppointment());
        ChangeAppointmentCommand = new DelegateCommand(o=>ChangeAppointmentCom());
    }

    private void ChangeAppointmentCom()
    {
        AppointmentViewModel sel = SelectedAppointment;
        if (sel != null)
        {

        }
        else
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);


    }

    public void NewAppointment()
    {
        var window = new MakeAppointmentView(_doctorRepository, _controller, Appointments, _patient);
        window.Show();
    }
    
}