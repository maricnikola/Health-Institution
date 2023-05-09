using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Room;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.ViewModels.DirectorViewModel;

namespace ZdravoCorp.Core.ViewModels;

public class PatientTableViewModel :ViewModelBase  
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

    public PatientTableViewModel(User user, DoctorRepository doctorRepository,PatientRepository patientRepository )
    {
        _doctorRepository = doctorRepository;
        _doctor = _doctorRepository.GetDoctorByEmail(user.Email);
        _patientRepository = patientRepository;
        List<Patient> patinets = _patientRepository.Patients;

        _Allpatients = new ObservableCollection<PatientsViewModel>();
        foreach(Patient patient in patinets)
        {
            _Allpatients.Add(new PatientsViewModel(patient));
        }
        _patients = _Allpatients;
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
}
