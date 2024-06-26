﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services.Services;
using ZdravoCorp.Core.HospitalSystem.Notifications.Services;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.PatientFiles.Presriptions.Models;
using ZdravoCorp.Core.PatientFiles.Presriptions.Services;
using ZdravoCorp.Core.Scheduling.Models;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.GUI.HospitalAssets.Equipment.ViewModels;
using ZdravoCorp.GUI.HospitalAssets.Equipment.Views;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.PatientFiles.Prescriptions.ViewModels;

public class CreatePrescriptionsViewModel : ViewModelBase
{
    public ObservableCollection<PrescriptionViewModel> Prescriptions { get; }
    private List<Prescription> _prescriptions { get; }

    public PrescriptionViewModel SelectedPrescription { get; set; }
    private IScheduleService _scheduleService;
    private Appointment _appointment;
    private IInventoryService _inventoryService;
    private IRoomService _roomService;
    private IMedicamentService _medicamentService;
    private IPatientService _patientService;
    private INotificationService _notificationService;
    private Anamnesis _anamnesis;
    private List<int> _hourlyRates = new List<int>();
    public CreatePrescriptionsViewModel(Appointment appointment,IScheduleService scheduleService,
        IInventoryService inventoryService,IRoomService roomService,Anamnesis anamnesis)
    {
        _medicamentService = Injector.Container.Resolve<IMedicamentService>();
        _patientService = Injector.Container.Resolve<IPatientService>();
        _notificationService = Injector.Container.Resolve<INotificationService>();
        
        _anamnesis = anamnesis;
        _prescriptions = new List<Prescription>();
        Prescriptions = new ObservableCollection<PrescriptionViewModel>();
        PossibleTimes = new[] { 1, 2, 3, 4, 5, 6 };
        PossibleInstructions = new[] { "BeforeMeal", "AfterMeal", "DuringMeal", "NotImportant" };
        PossibleMedicaments = _medicamentService.GetAllMedicamentNames().ToArray();
        //PossibleMedicaments = new[] { "paracetamol", "probiotik", "antibiotik", "brufen" };
        _inventoryService = inventoryService;
        _roomService = roomService;
        _appointment = appointment;
        _scheduleService = scheduleService;
        Add = new DelegateCommand(o => AddPrescription());
        Delete = new DelegateCommand(o => DeletePrescription());
        Save = new DelegateCommand(o => SavePerformingAppointment());
    }

    public ICommand Save { get; }
    public ICommand Add { get; }
    public ICommand Delete { get; }

    
    public int[] PossibleTimes { get; }
    public string[] PossibleInstructions { get; }
    public string[] PossibleMedicaments { get; }

    private int _selectedTime;
    public int SelectedTime
    {
        get
        {
            return _selectedTime;
        }
        set
        {
            _selectedTime = value;
            OnPropertyChanged(nameof(SelectedTime));
        }
    }
    private string _selectedInstruction;
    public string SelectedInstruction
    {
        get
        {
            return _selectedInstruction;
        }
        set
        {
            _selectedInstruction = value;
            OnPropertyChanged(nameof(SelectedInstruction));
        }
    }
    private string  _selectedMedicament;
    public string  SelectedMedicament
    {
        get
        {
            return _selectedMedicament;
        }
        set
        {
            _selectedMedicament = value;
            OnPropertyChanged(nameof(SelectedMedicament));
        }
    }
    private DateTime _expirationDate = DateTime.Today;
    public DateTime ExpirationDate
    {
        get
        {
            return _expirationDate;
        }
        set
        {
            if (value < DateTime.Today)
            {
                MessageBox.Show("Select a date in the future.", "Error", MessageBoxButton.OK);
                _expirationDate = DateTime.Today;
            }
            else
            {
                _expirationDate = value;
            }
            OnPropertyChanged(nameof(ExpirationDate));
        }
    }
    private string _hours;
    public string Hours
    {
        get
        {
            return _hours;
        }
        set
        {
            _hours = value;
            OnPropertyChanged(nameof(Hours));
        }
    }


    public void AddPrescription()
    {
        if (!CheckPrescriptionData())
        {
            MessageBox.Show("Invalid data for prescription", "Error", MessageBoxButton.OK);
            return;
        }

        var prescriptoion = new Prescription(SelectedMedicament, SelectedTime, SelectedInstruction,ExpirationDate,_hourlyRates);
        var PrescriptionModel = new PrescriptionViewModel(prescriptoion);
        foreach (var prescription in Prescriptions)
        {
            if (PrescriptionModel.Medicament == prescription.Medicament)
            {
                MessageBox.Show("Medicament already exists", "Error", MessageBoxButton.OK);
                return;
            }
        }
        var components  = _medicamentService.GetByName(prescriptoion.Medicament).Components;
        foreach (var allergen in _anamnesis.Allergens)
        {
            if (components.Contains(allergen))
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to add a prescription even " +
                    "though the patient is allergic?", "Error", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
        }
        _prescriptions.Add(prescriptoion);
        Prescriptions.Add(PrescriptionModel);
  

    }
    private bool CheckPrescriptionData()
    {
        if (ExpirationDate == DateTime.Today || SelectedInstruction == null || SelectedMedicament == null
            || SelectedTime == null) return false;
        try
        {
            List<string> hourlyRates = Hours.Split(",").ToList();
            _hourlyRates = hourlyRates.Select(s => int.Parse(s)).ToList();
            if (hourlyRates.Count != SelectedTime) return false;
            if (_hourlyRates.Count != _hourlyRates.Distinct().Count()) return false;
            if (!_hourlyRates.All(num => num <= 23 && num >= 0)) return false; 
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }
    public void DeletePrescription()
    {
        if(SelectedPrescription == null)
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
            return;
        }
        foreach(Prescription prescription in _prescriptions)
        {
            if(SelectedPrescription.Instructions == prescription.Instructions && SelectedPrescription.Medicament == prescription.Medicament
                && SelectedPrescription.TimesADay == prescription.TimesADay){
                _prescriptions.Remove(prescription);
                break;
            }
        }
        Prescriptions.Remove(SelectedPrescription);
    }

    public void SavePerformingAppointment()
    {
        if(Prescriptions.Count == 0)
        {
            MessageBox.Show("No prescription added", "Error", MessageBoxButton.OK);
            return;
        }
        CloseWindow();
        _scheduleService.ChangePerformingAppointment(_appointment.Id,_anamnesis,_prescriptions, (int)_appointment.Room);

        var patient = _patientService.GetByEmail(_appointment.PatientEmail);
        _notificationService.CreateNotificationsFromPrescriptions(_prescriptions,patient.NotificationTime,patient.Email);
        ShowDEquipmentSpentDialog();
    }
    public void ShowDEquipmentSpentDialog()
    {
        var window = new DynamicEquipmentSpentView
        { DataContext = new DynamicEquipmentSpentViewModel(_inventoryService, _roomService, (int)_appointment.Room) };
        window.Show();
    }
    private void CloseWindow()
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }

}
