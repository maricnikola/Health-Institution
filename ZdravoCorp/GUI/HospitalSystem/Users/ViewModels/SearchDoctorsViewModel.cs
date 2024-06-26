﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.GUI.Main;
using ZdravoCorp.GUI.Scheduling.ViewModels;
using ZdravoCorp.GUI.Scheduling.Views;
using static ZdravoCorp.Core.HospitalSystem.Users.Models.Doctor;

namespace ZdravoCorp.GUI.HospitalSystem.Users.ViewModels;

public class SearchDoctorsViewModel : ViewModelBase
{
    private readonly IDoctorService _doctorService;
    private readonly IScheduleService _scheduleService;
    private Patient _patient;
    private ObservableCollection<DrViewModel> _doctors;
    private ObservableCollection<DrViewModel> _allDoctors;
    private ObservableCollection<DrViewModel> _filteredDoctors; 
    private string _firstNameSearchText = "";
    private string _lastNameSearchText = "";
    private string _selectedSpecialization = "None";
    private readonly IRoomService _roomService;

    public HashSet<string> PossibleSpecializations { get; }
    public DrViewModel SelectedDoctor { get; set; }

    public ICommand CreateAppointmentWithSelectedDoctorCommand { get; set; }

    public SearchDoctorsViewModel(IDoctorService doctorService, IScheduleService scheduleService, Patient patient,IRoomService roomService)
    {
        _roomService = roomService;
        _doctorService = doctorService;
        _scheduleService = scheduleService;
        _patient = patient;
        _allDoctors = new ObservableCollection<DrViewModel>();
        foreach (var doctor in _doctorService.GetAll()) _allDoctors.Add(new DrViewModel(doctor));
        _doctors = _allDoctors;
        PossibleSpecializations = new HashSet<string>();
        LoadComboBoxColleciton();

        CreateAppointmentWithSelectedDoctorCommand = new DelegateCommand(o => CreateAppointmentWithSelectedDoctor());
    }

    public IEnumerable<DrViewModel> Doctors
    {
        get => _doctors;
        set
        {
            _doctors = new ObservableCollection<DrViewModel>(value);
            OnPropertyChanged();
        }
    }

    public string FirstNameSearchBox
    {
        get => _firstNameSearchText;
        set
        {
            _firstNameSearchText = value;
            UpdateTable();
            OnPropertyChanged("SearchFirstName");
        }
    }

    public string LastNameSearchBox
    {
        get => _lastNameSearchText;
        set
        {
            _lastNameSearchText = value;
            UpdateTable();
            OnPropertyChanged("SearchLastName");
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


    private void UpdateTable()
    {
        _filteredDoctors = _allDoctors;
        var f1 = _filteredDoctors.Intersect(UpdateTableFromFirstNameSearch());
        var f2 = f1.Intersect(UpdateTableFromLastNameSearch());
        var f3 = f2.Intersect(UpdateTableFromSpecialization());
        Doctors = f3;
    }

    private ObservableCollection<DrViewModel> UpdateTableFromFirstNameSearch()
    {
        if (_firstNameSearchText != "")
            return new ObservableCollection<DrViewModel>(SearchByFirstName(_firstNameSearchText));
        return _allDoctors;
    }
    private ObservableCollection<DrViewModel> UpdateTableFromLastNameSearch()
    {
        if (_lastNameSearchText != "")
            return new ObservableCollection<DrViewModel>(SearchByLastName(_lastNameSearchText));
        return _allDoctors;
    }

    private ObservableCollection<DrViewModel> UpdateTableFromSpecialization()
    {
        if (_selectedSpecialization != "None")
            return new ObservableCollection<DrViewModel>(FilterBySpecialization(_selectedSpecialization));
        return _allDoctors;
    }

    private IEnumerable<DrViewModel> SearchByFirstName(string inputText)
    {
        return _allDoctors.Where(item => item.DoctorsName.ToLower().Contains(inputText.ToLower()));
    }
    private IEnumerable<DrViewModel> SearchByLastName(string inputText)
    {
        return _allDoctors.Where(item => item.DoctorLastName.ToLower().Contains(inputText.ToLower()));
    }
    private IEnumerable<DrViewModel> FilterBySpecialization(string specialization)
    {
        return _allDoctors.Where(item => item.Specialization == specialization);
    }

    private void LoadComboBoxColleciton()
    {
        PossibleSpecializations.Add("None");
        PossibleSpecializations.Add(SpecializationType.Surgeon.ToString());
        PossibleSpecializations.Add(SpecializationType.Psychologist.ToString());
        PossibleSpecializations.Add(SpecializationType.Neurologist.ToString());
        PossibleSpecializations.Add(SpecializationType.Urologist.ToString());
        PossibleSpecializations.Add(SpecializationType.Anesthesiologist.ToString());
    }

    private void  CreateAppointmentWithSelectedDoctor()
    {
        if (SelectedDoctor != null)
        {
            var window = new MakeAppointmentView
            {
                DataContext = new MakeAppointmentViewModel(_scheduleService,
                    new ObservableCollection<AppointmentViewModel>(),
                    _doctorService, _patient, SelectedDoctor.Email,_roomService)
            };
            //var window = new MakeAppointmentView(_doctorRepository, _scheduleService, Appointments, _patient);
            window.Show();
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }
}