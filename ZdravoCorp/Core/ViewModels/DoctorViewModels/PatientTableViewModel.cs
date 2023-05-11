using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.MedicalRecord;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.MedicalRecord;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.ViewModels.DirectorViewModel;
using ZdravoCorp.View;
using ZdravoCorp.View.DoctorView;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class PatientTableViewModel : ViewModelBase
{
    private ObservableCollection<PatientsViewModel> _patients;
    private ObservableCollection<PatientsViewModel> _Allpatients;
    private ObservableCollection<PatientsViewModel> _searchedPatients;
    

    private Doctor _doctor;
    // public ObservableCollection<PatientsViewModel> Patients => _patients; 
    private string _searchText = "";

    public PatientsViewModel SelectedPatient { get; set; }
    private PatientRepository _patientRepository;
    private DoctorRepository _doctorRepository;
    private MedicalRecordRepository _medicalRecordRepository;
    private ScheduleRepository _scheduleRepository;

    public ICommand ChangeMedicalRecordCommand { get; }

    public PatientTableViewModel(User user,ScheduleRepository scheduleRepository, DoctorRepository doctorRepository, PatientRepository patientRepository, MedicalRecordRepository medicalRecordRepository)
    {
        _scheduleRepository = scheduleRepository;
        _doctorRepository = doctorRepository;
        _doctor = _doctorRepository.GetDoctorByEmail(user.Email);
        _patientRepository = patientRepository;
        _medicalRecordRepository = medicalRecordRepository;

        List<Patient> patinets = _patientRepository.Patients;

        _Allpatients = new ObservableCollection<PatientsViewModel>();
        foreach (Patient patient in patinets)
        {
            _Allpatients.Add(new PatientsViewModel(patient));
        }
        _patients = _Allpatients;

        ChangeMedicalRecordCommand = new DelegateCommand(o => OpenMedicalRecordChange());
    }

    public string SearchBox
    {
        get { return _searchText; }
        set
        {
            _searchText = value;
            UpdateTable();
            OnPropertyChanged("Search");
        }
    }

    public IEnumerable<PatientsViewModel> Patients
    {
        get
        {
            return _patients;

        }
        set
        {

            _patients = new ObservableCollection<PatientsViewModel>(value);
            OnPropertyChanged();
        }
    }

    private ObservableCollection<PatientsViewModel> UpdateTableFromSearch()
    {
        if (_searchText != "")
        {
            return new ObservableCollection<PatientsViewModel>(Search(_searchText));

        }
        else
        {

            return _patients;
        }
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
        PatientsViewModel patient = SelectedPatient;
        if (patient != null )
        {
            Patient _patient = _patientRepository.GetPatientByEmail(patient.Email);
            bool isExamined = _scheduleRepository.IsPatientExamined(_patient,_doctor);
            if (isExamined)
            {
            MedicalRecord medicalR = _medicalRecordRepository.GetById(patient.Email);
            ChangeMedicalRecordView window = new ChangeMedicalRecordView() { DataContext = new MedicalRecordViewModel(medicalR,_medicalRecordRepository) };
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
