﻿using System;
using System.Linq;
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
using ZdravoCorp.GUI.HospitalAssets.Equipment.ViewModels;
using ZdravoCorp.GUI.HospitalAssets.Equipment.Views;
using ZdravoCorp.GUI.Main;
using ZdravoCorp.GUI.PatientFiles.MedicalRecords.ViewModels;
using ZdravoCorp.GUI.PatientFiles.MedicalRecords.Views;
using ZdravoCorp.GUI.PatientFiles.Prescriptions.ViewModels;
using ZdravoCorp.GUI.PatientFiles.Prescriptions.Views;
using ZdravoCorp.GUI.PatientFiles.Refferals.ViewModels;
using ZdravoCorp.GUI.PatientFiles.Refferals.Views;

namespace ZdravoCorp.GUI.Scheduling.ViewModels;

public class PerformAppointmentViewModel : ViewModelBase
{
    private string _allergens;
    private readonly Appointment _appointment;
    private AppointmentViewModel _appointmentViewModel;
    private readonly IInventoryService _inventoryService;

    private string _keyWord;
    private ISpecialistsRefferalService _specialistsRefferalService;
    private readonly IMedicalRecordService _medicalRecordService;

    private string _opinion;
    private readonly Patient? _patient;
    private IPatientService _patientService;
    private int _roomId;
    private readonly IRoomService _roomService;
    private readonly IScheduleService _scheduleService;
    private readonly IDoctorService _doctorService;
    private readonly IHospitalRefferalService _hospitalRefferalService;
    private Anamnesis _anamnesis;


    private string _symptoms;


    public PerformAppointmentViewModel(Appointment performingAppointment, IScheduleService scheduleService,
        IPatientService patientService, IMedicalRecordService medicalRecordService,IInventoryService inventoryService, 
        IRoomService roomService,IDoctorService doctorService,IHospitalRefferalService hospitalRefferalService)
    {
        _hospitalRefferalService = hospitalRefferalService;
        _doctorService = doctorService;
        _roomService = roomService;
        _specialistsRefferalService = Injector.Container.Resolve<ISpecialistsRefferalService>();
        _inventoryService = inventoryService;
        _appointment = performingAppointment;
        _medicalRecordService = medicalRecordService;
        _scheduleService = scheduleService;
        _patientService = patientService;
        _patient = _patientService.GetByEmail(performingAppointment.PatientEmail);

        CancelCommand = new DelegateCommand(o => CloseWindow());
        MedicalRCommand = new DelegateCommand(o => ShowMedicalRecordDialog());
        NextCommand = new DelegateCommand(o => NextToPrescription());
        SpecialistsRefferal = new DelegateCommand(o => ShowSpecialistsRefferal());
        HospitalRefferal = new DelegateCommand(o => ShowHospitalRefferal());
        
    }

    public string PatientMail => _patient.Email;
    public string PatientName => _patient.FullName;
    public ICommand NextCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand MedicalRCommand { get; }
    public ICommand SpecialistsRefferal { get; }
    public ICommand HospitalRefferal { get; }

    public string Symptoms
    {
        get => _symptoms;
        set
        {
            _symptoms = value;
            OnPropertyChanged();
        }
    }

    public string Opinion
    {
        get => _opinion;
        set
        {
            _opinion = value;
            OnPropertyChanged();
        }
    }

    public string Allergens
    {
        get => _allergens;
        set
        {
            _allergens = value;
            OnPropertyChanged();
        }
    }

    public string KeyWord
    {
        get => _keyWord;
        set
        {
            _keyWord = value;
            OnPropertyChanged();
        }
    }


    private void CloseWindow()
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }

    public void ShowMedicalRecordDialog()
    {
        if (_patient != null)
        {
            var medicalR = _medicalRecordService.GetById(_patient.Email);
            var window = new ChangeMedicalRecordView
                { DataContext = new MedicalRecordViewModel(medicalR, _medicalRecordService) };
            window.Show();
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }

    public void ShowDEquipmentSpentDialog()
    {
        var window = new DynamicEquipmentSpentView
            { DataContext = new DynamicEquipmentSpentViewModel(_inventoryService, _roomService, (int)_appointment.Room) };
        window.Show();
    }


    public void NextToPrescription()
    {
        try
        {
            var patientSymptoms = Symptoms.Trim().Split(",").ToList();
            var patientAllergens = Allergens.Trim().Split(",").ToList();
            var doctorOpinion = Opinion.Trim();
            var anamnesisKeyWord = KeyWord;
            if (_scheduleService.CheckPerformingAppointmentData(patientSymptoms, doctorOpinion, patientAllergens,
                    anamnesisKeyWord))
            {
                _anamnesis = new Anamnesis(patientSymptoms, doctorOpinion, anamnesisKeyWord, patientAllergens);
                CloseWindow();
                ShowPrescription();
     
            }
            else
            {
                MessageBox.Show("Invalid data for performing", "Error", MessageBoxButton.OK);
            }
        }
        catch (Exception)
        {
            MessageBox.Show("Invalid data for performing", "Error", MessageBoxButton.OK);
        }
    }

    public void ShowSpecialistsRefferal()
    {
        CloseWindow();
        var window = new AddSpecialistsRefferalView() { DataContext = new AddSpecialistsRefferalViewModel(this,_doctorService,_appointment.Doctor,_patient,_scheduleService, _appointment)};
        window.Show();
    }

    public void ShowHospitalRefferal()
    {
        CloseWindow();
        var window = new AddHospitalRefferalView() { DataContext = new AddHospitalRefferalViewModel(this,_appointment,_scheduleService,_hospitalRefferalService,_roomService) };
        window.Show();
    }
    public void ShowPrescription()
    {
        var window = new CreatePrescriptionsView() { DataContext = new CreatePrescriptionsViewModel(_appointment,_scheduleService,_inventoryService,_roomService,_anamnesis) };
        window.Show();
    }

}