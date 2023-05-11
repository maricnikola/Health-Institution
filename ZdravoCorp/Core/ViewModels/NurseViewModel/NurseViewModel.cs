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
    private MedicalRecordRepository _medicalRecordRepository;
    private ScheduleRepository _scheduleRepository;
    private DoctorRepository _doctorRepository;
    private object _currentView;

    public ICommand LoadEquipmentCommand { get; private set; }
    public ICommand LoadDynamicEquipmentCommand { get; private set; }


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

    public NurseViewModel(MedicalRecordRepository medicalRecordRepository, ScheduleRepository scheduleRepository, DoctorRepository doctorRepository)
    {
        _medicalRecordRepository = medicalRecordRepository;
        _scheduleRepository = scheduleRepository;   
        _doctorRepository = doctorRepository;   
        LoadEquipmentCommand = new DelegateCommand(o => NewPatientReception());
        LoadDynamicEquipmentCommand = new DelegateCommand(o => UrgentAppointmentReservation());
        _currentView = new PatientReceptionViewModel();
    }

    public void NewPatientReception()
    {
        CurrentView = new PatientReceptionViewModel();
    }

    public void UrgentAppointmentReservation()
    {
        CurrentView = new UrgentAppointmentViewModel(_medicalRecordRepository, _scheduleRepository, _doctorRepository);
    }
}