using Autofac;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.MedicalRecordRepo;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.Services.DoctorServices;
using ZdravoCorp.Core.Services.MedicalRecordServices;
using ZdravoCorp.Core.Services.PatientServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.ViewModels.NurseViewModel;

public class NurseViewModel : ViewModelBase

{
    private object _currentView;
    private readonly IDoctorService _doctorService;
    private readonly IMedicalRecordService _medicalRecordService;
    private readonly IPatientService _patientService;
    private readonly IScheduleService _scheduleService;

    public NurseViewModel()
    {

        _patientService = Injector.Container.Resolve<IPatientService>();
        _medicalRecordService = Injector.Container.Resolve<IMedicalRecordService>();
        _scheduleService = Injector.Container.Resolve<IScheduleService>();
        _doctorService = Injector.Container.Resolve<IDoctorService>();
        
        
        NewPatientReceptionCommand = new DelegateCommand(o => NewPatientReception());
        UrgentAppointmentReservationCommand = new DelegateCommand(o => UrgentAppointmentReservation());
        _currentView = new PatientReceptionViewModel(_patientService, _scheduleService);
    }

    public ICommand NewPatientReceptionCommand { get; private set; }
    public ICommand UrgentAppointmentReservationCommand { get; private set; }


    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public void NewPatientReception()
    {
        CurrentView = new PatientReceptionViewModel(_patientService, _scheduleService);
    }

    public void UrgentAppointmentReservation()
    {
        CurrentView = new UrgentAppointmentViewModel(_medicalRecordService, _scheduleService, _doctorService);
    }
}