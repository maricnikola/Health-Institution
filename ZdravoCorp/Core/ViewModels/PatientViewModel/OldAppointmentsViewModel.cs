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
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.ViewModels.DirectorViewModel;
using ZdravoCorp.View.PatientV;
using ZdravoCorp.View.PatientView;
using static ZdravoCorp.Core.Models.Users.Doctor;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class OldAppointmentsViewModel : ViewModelBase
{
    private ObservableCollection<AppointmentViewModel> _appointments;
    private readonly ObservableCollection<AppointmentViewModel> _allAppointments;
    private ObservableCollection<AppointmentViewModel> _filteredAppointments;
    private HashSet<string> _possibleDoctors;
    private HashSet<string> _possibleSpecializations;
    //public ObservableCollection<AppointmentViewModel> Appointments => _appointments;
    public List<Appointment> CompleteAppointments;
    public AppointmentViewModel SelectedAppointment { get; set; }
    private ScheduleRepository _scheduleRepository;
    private DoctorRepository _doctorRepository;
    private Patient _patient;
    private string _searchText = "";
    private string _selectedDoctor = "None";
    private string _selectedSpecialization = "None";

    public ICommand ViewAnamnesisCommand { get; set; }

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
    public string SelectedDoctor
    {
        get { return _selectedDoctor; }
        set
        {
            _selectedDoctor = value;
            UpdateTable();
            OnPropertyChanged("SelectedDoctor");
        }
    }
    public string SelectedSpecialization
    {
        get { return _selectedSpecialization; }
        set
        {
            _selectedSpecialization = value;
            UpdateTable();
            OnPropertyChanged("SelectedSpecialization");
        }
    }

    public IEnumerable<AppointmentViewModel> Appointments
    {
        get { return _appointments; }
        set
        {
            _appointments = new ObservableCollection<AppointmentViewModel>(value);
            OnPropertyChanged();
        }
    }

    public HashSet<string> PossibleDoctors
    {
        get { return _possibleDoctors; }
    }
    public HashSet<string> PossibleSpecializations
    {
        get { return _possibleSpecializations; }
    }
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
        {
            return new ObservableCollection<AppointmentViewModel>(Search(_searchText));
        }
        else
        {
            return _allAppointments;
        }
    }
    private ObservableCollection<AppointmentViewModel> UpdateTableFromDoctor()
    {
        if (_selectedDoctor != "None")
        {
            return new ObservableCollection<AppointmentViewModel>(FilterByDoctor(_selectedDoctor));
        }
        else
        {
            return _allAppointments;
        }
    }
    private ObservableCollection<AppointmentViewModel> UpdateTableFromSpecialization()
    {
        if (_selectedSpecialization != "None")
        {
            return new ObservableCollection<AppointmentViewModel>(FilterBySpecialization(_selectedSpecialization));
        }
        else
        {
            return _allAppointments;
        }
    }

    public IEnumerable<AppointmentViewModel> Search(string inputText)
    {
        return _allAppointments.Where(item => item.Anamnesis.ToLower().Contains(inputText.ToLower()));
    }

    public IEnumerable<AppointmentViewModel> FilterByDoctor(string doctor)
    {
        return _allAppointments.Where(item => item.DoctorName== doctor);
    }

    public IEnumerable<AppointmentViewModel> FilterBySpecialization(string specialization)
    {
        return _allAppointments.Where(item => item.Specialization == specialization);
    }

    public OldAppointmentsViewModel(ScheduleRepository scheduleRepository,
        DoctorRepository doctorRepository, Patient patient)
    {
        _patient = patient;
        _scheduleRepository = scheduleRepository;
        _allAppointments = new ObservableCollection<AppointmentViewModel>();
        _doctorRepository = doctorRepository;
        CompleteAppointments = _scheduleRepository.GetPatientsOldAppointments(_patient.Email);
        _possibleDoctors = new HashSet<string>();
        _possibleDoctors.Add("None");
        _possibleSpecializations = new HashSet<string>();
        _possibleSpecializations.Add("None");
        LoadComboBoxCollecitons();
        foreach (var appointment in CompleteAppointments)
        {
            _allAppointments.Add(new AppointmentViewModel(appointment));
            _possibleDoctors.Add(appointment.Doctor.FullName);
        }

        _appointments = _allAppointments;
        ViewAnamnesisCommand = new DelegateCommand(o => ViewAnamnesisComm());
    }

    public void ViewAnamnesisComm()
    {
        if (SelectedAppointment != null)
        {
            Appointment? selectedAppointment = null;
            foreach (var appointment in CompleteAppointments.Where(appointment => appointment.Id == SelectedAppointment.Id))
            {
                selectedAppointment = appointment;
            }

            var window = new FullAnamnesisView()
            {
                DataContext = new FullAnamnesisViewModel(selectedAppointment.Anamnesis)
            };
            window.Show();
        }
        else
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
    }

    public void LoadComboBoxCollecitons()
    {
        _possibleSpecializations.Add("None");
        _possibleSpecializations.Add(SpecializationType.Surgeon.ToString());
        _possibleSpecializations.Add(SpecializationType.Psychologist.ToString());
        _possibleSpecializations.Add(SpecializationType.Neurologist.ToString());
        _possibleSpecializations.Add(SpecializationType.Urologist.ToString());
        _possibleSpecializations.Add(SpecializationType.Anesthesiologist.ToString());
    }

}