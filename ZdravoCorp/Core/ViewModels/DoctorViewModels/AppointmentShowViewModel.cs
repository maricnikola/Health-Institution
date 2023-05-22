﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.MedicalRecordRepo;
using ZdravoCorp.Core.Repositories.RoomRepo;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.Services.PatientServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Services.MedicalRecordServices;
using ZdravoCorp.View;
using ZdravoCorp.View.DoctorView;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class AppointmentShowViewModel : ViewModelBase
{
    private readonly ObservableCollection<MedicalRecordViewModel> _medicalRecords;

    private DateTime _dateAppointment = DateTime.Now + TimeSpan.FromHours(1);
    private readonly Doctor _doctor;
    private readonly DoctorRepository _doctorRepository;
    private readonly InventoryRepository _inventoryRepository;
    private readonly IMedicalRecordService _medicalRecordService;
    private readonly IPatientService _patientService;
    private readonly RoomRepository _roomRepository;
    private readonly IScheduleService _scheduleService;
    private int counterViews;

    public AppointmentShowViewModel(User user, ScheduleRepository scheduleRepository, DoctorRepository doctorRepository,
        IPatientService patientService, IMedicalRecordService medicalRecordService,
        InventoryRepository inventoryRepository, RoomRepository roomRepository)
    {
        counterViews = 0;
        _patientService = patientService;
        _scheduleService = scheduleService;
        _inventoryRepository = inventoryRepository;
        _roomRepository = roomRepository;

        _doctorRepository = doctorRepository;
        _doctor = _doctorRepository.GetByEmail(user.Email);

        var appointments = _scheduleRepository.GetDoctorAppointments(_doctor.Email);
        _medicalRecordService = medicalRecordService;

        Appointments = new ObservableCollection<AppointmentViewModel>();

        AddAppointmentCommand = new DelegateCommand(o => OpenAddDialog());
        ChangeAppointmentCommand = new DelegateCommand(o => OpenChangeDialog());
        CancelAppointmentCommand = new DelegateCommand(o => CancelAppointment());
        SearchAppointmentCommand = new DelegateCommand(o => SearchAppointments());
        ViewMedicalRecordCommand = new DelegateCommand(o => ShowMedicalRecord());
        PerformAppointmentCommand = new DelegateCommand(o => ShowPerformingView());
    }

    public ObservableCollection<AppointmentViewModel> Appointments { get; }

    public AppointmentViewModel SelectedAppointments { get; set; }
    public ICommand ChangeAppointmentCommand { get; }
    public ICommand AddAppointmentCommand { get; }
    public ICommand CancelAppointmentCommand { get; }
    public ICommand SearchAppointmentCommand { get; }
    public ICommand ViewMedicalRecordCommand { get; }
    public ICommand PerformAppointmentCommand { get; }

    public DateTime DateAppointment
    {
        get => _dateAppointment;
        set
        {
            _dateAppointment = value;
            if (_dateAppointment < DateTime.Today)
            {
                MessageBox.Show("Select date in future", "Error", MessageBoxButton.OK);
                _dateAppointment = DateTime.Today;
                return;
            }

            OnPropertyChanged();
        }
    }

    public void OpenAddDialog()
    {
        var addAp = new AddAppointmentView
        {
            DataContext = new AddAppointmentViewModel(_scheduleRepository, _doctorRepository, Appointments,
                _patientService, _doctor, _medicalRecordService, _dateAppointment)
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
                    _doctorRepository, Appointments, _patientService, _doctor, patient, appointmentViewModel,
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
                appointment.PatientEmail, appointment.Anamnesis.KeyWord);
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
                    DataContext = new PerformAppointmentViewModel(appointmentPerforming, _scheduleRepository,
                        _patientService, _medicalRecordService, _inventoryRepository, _roomRepository)
                };
                window.Show();
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