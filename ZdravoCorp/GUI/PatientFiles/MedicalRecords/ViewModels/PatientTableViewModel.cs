using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.PatientFiles.MedicalRecords.Services;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.GUI.HospitalSystem.Users.ViewModels;
using ZdravoCorp.GUI.Main;
using ZdravoCorp.GUI.PatientFiles.MedicalRecords.Views;

namespace ZdravoCorp.GUI.PatientFiles.MedicalRecords.ViewModels;

public class PatientTableViewModel : ViewModelBase
{
    private readonly ObservableCollection<PatientsViewModel> _Allpatients;


    private readonly Doctor _doctor;
    private readonly IDoctorService _doctorService;
    private readonly IMedicalRecordService _medicalRecordService;
    private readonly IPatientService _patientService;
    private ObservableCollection<PatientsViewModel> _patients;
    private readonly IScheduleService _scheduleService;

    private ObservableCollection<PatientsViewModel> _searchedPatients;

    // public ObservableCollection<PatientsViewModel> Patients => _patients; 
    private string _searchText = "";

    public PatientTableViewModel(User user, IScheduleService scheduleService, IDoctorService doctorService,
        IPatientService patientService, IMedicalRecordService medicalRecordService)
    {
        _scheduleService = scheduleService;
        _doctorService = doctorService;
        _doctor = _doctorService.GetByEmail(user.Email);
        _patientService = patientService;
        _medicalRecordService = medicalRecordService;

        var patinets = _patientService.GetAll();

        _Allpatients = new ObservableCollection<PatientsViewModel>();
        foreach (var patient in patinets) _Allpatients.Add(new PatientsViewModel(patient));
        _patients = _Allpatients;

        ChangeMedicalRecordCommand = new DelegateCommand(o => OpenMedicalRecordChange());
    }

    public PatientsViewModel SelectedPatient { get; set; }

    public ICommand ChangeMedicalRecordCommand { get; }

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

    public IEnumerable<PatientsViewModel> Patients
    {
        get => _patients;
        set
        {
            _patients = new ObservableCollection<PatientsViewModel>(value);
            OnPropertyChanged();
        }
    }

    private ObservableCollection<PatientsViewModel> UpdateTableFromSearch()
    {
        if (_searchText != "")
            return new ObservableCollection<PatientsViewModel>(Search(_searchText));
        return _patients;
    }

    public IEnumerable<PatientsViewModel> Search(string inputText)
    {
        var p = _Allpatients.Where(patient => patient.ToString().Contains(inputText));
        return p;
    }


    private void UpdateTable()
    {
        _searchedPatients = _Allpatients;
        var f1 = _searchedPatients.Intersect(UpdateTableFromSearch());
        Patients = f1;
    }

    public void OpenMedicalRecordChange()
    {
        var patient = SelectedPatient;
        if (patient != null)
        {
            var _patient = _patientService.GetByEmail(patient.Email);
            var isExamined = _scheduleService.IsPatientExamined(_patient, _doctor);
            if (isExamined)
            {
                var medicalR = _medicalRecordService.GetById(patient.Email);
                var window = new ChangeMedicalRecordView
                    { DataContext = new MedicalRecordViewModel(medicalR, _medicalRecordService) };
                window.Show();
            }
            else
            {
                MessageBox.Show("Patient is not examined", "Error", MessageBoxButton.OK);
            }
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }
}