﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Autofac;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services.Services;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.PatientFiles.MedicalRecords.Services;
using ZdravoCorp.Core.PatientFiles.Refferals.Services;
using ZdravoCorp.Core.Scheduling.Models;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.GUI.Main;
using ZdravoCorp.GUI.PatientFiles.MedicalRecords.ViewModels;
using ZdravoCorp.GUI.PatientFiles.MedicalRecords.Views;
using ZdravoCorp.GUI.Scheduling.Views;

namespace ZdravoCorp.GUI.Scheduling.ViewModels;

public class AppointmentShowViewModel : ViewModelBase
{
    private readonly ObservableCollection<MedicalRecordViewModel> _medicalRecords;

    private DateTime _dateAppointment = DateTime.Now + TimeSpan.FromHours(1);
    private readonly Doctor _doctor;
    private readonly IDoctorService _doctorService;
    private readonly IInventoryService _inventoryService;
    private readonly IMedicalRecordService _medicalRecordService;
    private readonly IPatientService _patientService;
    private readonly IRoomService _roomService;
    private readonly IScheduleService _scheduleService;
    private readonly IHospitalRefferalService _hospitalRefferalService;
    private int counterViews;

    public AppointmentShowViewModel(User user, IScheduleService scheduleService, IDoctorService doctorService,
        IPatientService patientService, IMedicalRecordService medicalRecordService,
        IInventoryService inventoryService, IRoomService roomService)
    {
        _hospitalRefferalService = Injector.Container.Resolve<IHospitalRefferalService>();
        counterViews = 0;
        _patientService = patientService;
        _scheduleService = scheduleService;
        _inventoryService = inventoryService;
        _roomService = roomService;

        _doctorService = doctorService;
        _doctor = _doctorService.GetByEmail(user.Email);

        var appointments = _scheduleService.GetDoctorAppointments(_doctor.Email);
        _medicalRecordService = medicalRecordService;

        Appointments = new ObservableCollection<AppointmentViewModel>();

        AddAppointmentCommand = new DelegateCommand(o => OpenAddDialog());
        ChangeAppointmentCommand = new DelegateCommand(o => OpenChangeDialog());
        CancelAppointmentCommand = new DelegateCommand(o => CancelAppointment());
        ViewMedicalRecordCommand = new DelegateCommand(o => ShowMedicalRecord());
        PerformAppointmentCommand = new DelegateCommand(o => ShowPerformingView());
    }

    public ObservableCollection<AppointmentViewModel> Appointments { get; }

    public AppointmentViewModel SelectedAppointments { get; set; }
    public ICommand ChangeAppointmentCommand { get; }
    public ICommand AddAppointmentCommand { get; }
    public ICommand CancelAppointmentCommand { get; }
    public ICommand ViewMedicalRecordCommand { get; }
    public ICommand PerformAppointmentCommand { get; }

    public DateTime DateAppointment
    {
        get => _dateAppointment;
        set
        {
            _dateAppointment = value;

            OnPropertyChanged();
            SearchAppointments();
        }
    }

    public void OpenAddDialog()
    {
        var addAp = new AddAppointmentView
        {
            DataContext = new AddAppointmentViewModel(_scheduleService, _doctorService, Appointments,
                _patientService, _doctor, _medicalRecordService, _dateAppointment,_hospitalRefferalService,_roomService)
        };
        addAp.Show();
    }

    public void OpenChangeDialog()
    {
        var appointmentViewModel = SelectedAppointments;
        if (appointmentViewModel != null)
        {
            var appointment = _scheduleService.GetAppointmentById(appointmentViewModel.Id);
            if (appointment.Status)
            {
                MessageBox.Show("Appointment is performed", "Error", MessageBoxButton.OK);
                return;
            }

            var patientMail = appointmentViewModel.PatientMail;
            var patient = _patientService.GetByEmail(patientMail);
            var changeAp = new DrChangeAppointmentView
            {
                DataContext = new DrChangeAppointmentViewModel(SelectedAppointments, _scheduleService,
                    _doctorService, Appointments, _patientService, _doctor, patient, appointmentViewModel,
                    _dateAppointment)
            };
            changeAp.Show();
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }

    public void CancelAppointment()
    {
        var selectedAppointment = SelectedAppointments;
        if (selectedAppointment != null)
        {
            var appointment = _scheduleService.GetAppointmentById(selectedAppointment.Id);
            var appointmentDto = new AppointmentDTO(appointment.Id, appointment.Time.Start, appointment.Doctor,
                appointment.PatientEmail, null);
            if (appointment.Status)
            {
                MessageBox.Show("Appointment is performed", "Error", MessageBoxButton.OK);
                return;
            }

            var cancelAppointment = _scheduleService.CancelAppointmentByDoctor(appointmentDto);
            if (cancelAppointment != null)
                Appointments.Remove(GetById(selectedAppointment.Id, Appointments));
            else
                MessageBox.Show("Unable to cancel this appointment", "Error", MessageBoxButton.OK);
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }

    public AppointmentViewModel GetById(int id, ObservableCollection<AppointmentViewModel> Appointments)
    {
        foreach (var appointmentViewModel in Appointments)
            if (appointmentViewModel.Id == id)
                return appointmentViewModel;
        return null;
    }

    public void SearchAppointments()
    {
        var showAppointments = _scheduleService.GetAppointmentsForShow(_dateAppointment);
        Appointments.Clear();
        foreach (var appointment in showAppointments)
            if (!appointment.IsCanceled && appointment.Doctor.Email.Equals(_doctor.Email))
                Appointments.Add(new AppointmentViewModel(appointment));
    }

    public void ShowMedicalRecord()
    {
        var appointment = SelectedAppointments;
        if (appointment != null)
        {
            var medicalR = _medicalRecordService.GetById(appointment.PatientMail);
            var window = new MedicalRecordView
                { DataContext = new MedicalRecordViewModel(medicalR, _medicalRecordService) };
            window.Show();
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }

    public void ShowPerformingView()
    {
        var appointment = SelectedAppointments;
        if (appointment != null)
        {
            var checkPerformAppointment = _scheduleService.CanPerformAppointment(appointment.Id);
            var appointmentPerforming = _scheduleService.GetAppointmentById(appointment.Id);
            if (checkPerformAppointment && !appointmentPerforming.Status)
            {
                var window = new PerformAppointmentView
                {
                    DataContext = new PerformAppointmentViewModel(appointmentPerforming, _scheduleService,_patientService,
                    _medicalRecordService, _inventoryService, _roomService,_doctorService,_hospitalRefferalService)
                };
                window.Show();
                DateAppointment = DateTime.Now + TimeSpan.FromHours(1);
            }
            else
            {
                MessageBox.Show("Appointment cannot be performed", "Error", MessageBoxButton.OK);
            }
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }
}