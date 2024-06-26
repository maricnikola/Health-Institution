﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.Scheduling.Models;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.GUI.HospitalSystem.Analytics.ViewModels;
using ZdravoCorp.GUI.HospitalSystem.Analytics.Views;
using ZdravoCorp.GUI.Main;
using ZdravoCorp.GUI.Scheduling.Views;
using static ZdravoCorp.Core.HospitalSystem.Users.Models.Doctor;

namespace ZdravoCorp.GUI.Scheduling.ViewModels;

public class OldAppointmentsViewModel : ViewModelBase
{
    private readonly ObservableCollection<AppointmentViewModel> _allAppointments;
    private ObservableCollection<AppointmentViewModel> _appointments;
    private IDoctorService _doctorService;
    private ObservableCollection<AppointmentViewModel> _filteredAppointments;
    private readonly Patient _patient;
    private readonly IScheduleService _scheduleService;
    private string _searchText = "";
    private string _selectedDoctor = "None";

    private string _selectedSpecialization = "None";

    //public ObservableCollection<AppointmentViewModel> Appointments => _appointments;
    public List<Appointment> CompleteAppointments;

    public OldAppointmentsViewModel(IScheduleService scheduleService,
        IDoctorService doctorService, Patient patient)
    {
        _patient = patient;
        _scheduleService = scheduleService;
        _allAppointments = new ObservableCollection<AppointmentViewModel>();
        _doctorService = doctorService;
        CompleteAppointments = _scheduleService.GetPatientsOldAppointments(_patient.Email);
        PossibleDoctors = new HashSet<string>();
        PossibleDoctors.Add("None");
        PossibleSpecializations = new HashSet<string>();
        LoadComboBoxCollecitons();
        foreach (var appointment in CompleteAppointments)
        {
            _allAppointments.Add(new AppointmentViewModel(appointment));
            PossibleDoctors.Add(appointment.Doctor.FullName);
        }

        _appointments = _allAppointments;
        ViewAnamnesisCommand = new DelegateCommand(o => ViewAnamnesisComm());
        GradeDoctorCommand = new DelegateCommand(o => GradeDoctorComm());

    }

    public AppointmentViewModel SelectedAppointment { get; set; }

    public ICommand ViewAnamnesisCommand { get; set; }
    public ICommand GradeDoctorCommand { get; set; }

    public string SearchBox
    {
        get => _searchText;
        set
        {
            _searchText = value;
            UpdateTable();
            OnPropertyChanged("Search");
        }
    }

    public string SelectedDoctor
    {
        get => _selectedDoctor;
        set
        {
            _selectedDoctor = value;
            UpdateTable();
            OnPropertyChanged();
        }
    }

    public string SelectedSpecialization
    {
        get => _selectedSpecialization;
        set
        {
            _selectedSpecialization = value;
            UpdateTable();
            OnPropertyChanged();
        }
    }

    public IEnumerable<AppointmentViewModel> Appointments
    {
        get => _appointments;
        set
        {
            _appointments = new ObservableCollection<AppointmentViewModel>(value);
            OnPropertyChanged();
        }
    }

    public HashSet<string> PossibleDoctors { get; }

    public HashSet<string> PossibleSpecializations { get; }

    private void UpdateTable()
    {
        _filteredAppointments = _allAppointments;
        var f1 = _filteredAppointments.Intersect(UpdateTableFromSearch());
        var f2 = f1.Intersect(UpdateTableFromDoctor());
        var f3 = f2.Intersect(UpdateTableFromSpecialization());
        Appointments = f3;
    }

    private ObservableCollection<AppointmentViewModel> UpdateTableFromSearch()
    {
        if (_searchText != "")
            return new ObservableCollection<AppointmentViewModel>(Search(_searchText));
        return _allAppointments;
    }

    private ObservableCollection<AppointmentViewModel> UpdateTableFromDoctor()
    {
        if (_selectedDoctor != "None")
            return new ObservableCollection<AppointmentViewModel>(FilterByDoctor(_selectedDoctor));
        return _allAppointments;
    }

    private ObservableCollection<AppointmentViewModel> UpdateTableFromSpecialization()
    {
        if (_selectedSpecialization != "None")
            return new ObservableCollection<AppointmentViewModel>(FilterBySpecialization(_selectedSpecialization));
        return _allAppointments;
    }

    private IEnumerable<AppointmentViewModel> Search(string inputText)
    {
        return _allAppointments.Where(item => item.Anamnesis.ToLower().Contains(inputText.ToLower()));
    }

    private IEnumerable<AppointmentViewModel> FilterByDoctor(string doctor)
    {
        return _allAppointments.Where(item => item.DoctorName == doctor);
    }

    private IEnumerable<AppointmentViewModel> FilterBySpecialization(string specialization)
    {
        return _allAppointments.Where(item => item.Specialization == specialization);
    }

    public void ViewAnamnesisComm()
    {
        if (SelectedAppointment == null)
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
            return;
        }
        Appointment? selectedAppointment = null;
        foreach (var appointment in CompleteAppointments.Where(appointment =>
                     appointment.Id == SelectedAppointment.Id)) selectedAppointment = appointment;

        var window = new FullAnamnesisView
        {
            DataContext = new FullAnamnesisViewModel(selectedAppointment.Anamnesis)
        };
        window.Show();
        
    }

    public void GradeDoctorComm()
    {
        if (SelectedAppointment == null)
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
            return;
        }

        var window = new DoctorServayView
        {
            DataContext = new CreateDoctorSurveyViewModel(_doctorService, SelectedAppointment.DoctorEmail,
                _patient.Email)
        };
        window.Show();
    }

    public void LoadComboBoxCollecitons()
    {
        PossibleSpecializations.Add("None");
        PossibleSpecializations.Add(SpecializationType.Surgeon.ToString());
        PossibleSpecializations.Add(SpecializationType.Psychologist.ToString());
        PossibleSpecializations.Add(SpecializationType.Neurologist.ToString());
        PossibleSpecializations.Add(SpecializationType.Urologist.ToString());
        PossibleSpecializations.Add(SpecializationType.Anesthesiologist.ToString());
    }
}