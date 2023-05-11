using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.MedicalRecord;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.MedicalRecord;
using ZdravoCorp.Core.Repositories.Order;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;

namespace ZdravoCorp.Core.ViewModels.NurseViewModel;

public class NurseViewModel : ViewModelBase

{
    private PatientRepository _patientRepository;
    private MedicalRecordRepository _medicalRecordRepository;
    private ScheduleRepository _scheduleRepository;
    private DoctorRepository _doctorRepository;
    private object _currentView;

    public ICommand NewPatientReceptionCommand { get; private set; }
    public ICommand UrgentAppointmentReservationCommand { get; private set; }


    public object CurrentView
    {
        get
        {
            return _currentView;
        }
        set
        {
            _currentView = value;
            OnPropertyChanged("CurrentView");
        }
    }

    public NurseViewModel(MedicalRecordRepository medicalRecordRepository, ScheduleRepository scheduleRepository, DoctorRepository doctorRepository, PatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
        _medicalRecordRepository = medicalRecordRepository;
        _scheduleRepository = scheduleRepository;   
        _doctorRepository = doctorRepository;
        NewPatientReceptionCommand = new DelegateCommand(o => NewPatientReception());
        UrgentAppointmentReservationCommand = new DelegateCommand(o => UrgentAppointmentReservation());
        _currentView = new PatientReceptionViewModel(_patientRepository, _scheduleRepository);
    }

    public void NewPatientReception()
    {
        CurrentView = new PatientReceptionViewModel(_patientRepository, _scheduleRepository);
    }

    public void UrgentAppointmentReservation()
    {
        CurrentView = new UrgentAppointmentViewModel(_medicalRecordRepository, _scheduleRepository, _doctorRepository);
    }
}