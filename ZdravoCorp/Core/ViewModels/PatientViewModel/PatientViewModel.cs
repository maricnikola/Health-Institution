using System.Windows.Input;
using Autofac;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.MedicalRecordRepo;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.Services.DoctorServices;
using ZdravoCorp.Core.Services.MedicalRecordServices;
using ZdravoCorp.Core.Services.PatientServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class PatientViewModel : ViewModelBase
{
    private object _currentView;
    private readonly IDoctorService _doctorService;
    private readonly IMedicalRecordService _medicalRecordService;
    private readonly IPatientService _patientService;

    private readonly Patient _patient;
    private readonly IScheduleService _scheduleService;

    public PatientViewModel(User user)
    {
        //_scheduleRepository = _repositoryManager.ScheduleRepository;
        //_doctorRepository = _repositoryManager.DoctorRepository;
        //_patient = _patientService.GeTPatientByEmail(user.Email);
        //_medicalRecordRepository = _repositoryManager.MedicalRecordRepository;
        _doctorService = Injector.Container.Resolve<IDoctorService>();
        _medicalRecordService = Injector.Container.Resolve<IMedicalRecordService>();
        _scheduleService = Injector.Container.Resolve<IScheduleService>();
        _patientService = Injector.Container.Resolve<IPatientService>();

        _patient = _patientService.GetByEmail(user.Email);
        LoadAppointmentsCommand = new DelegateCommand(o => LoadAppointments());
        LoadMedicalRecordCommand = new DelegateCommand(o => LoadMedicalRecord());
        LoadOldAppointmentsCommand = new DelegateCommand(o => LoadOldAppointments());
        LoadDoctorsCommand = new DelegateCommand(o => LoadDoctors());
        _currentView = new AppointmentTableViewModel(_scheduleService, _doctorService, _patient);
    }

    public ICommand LoadAppointmentsCommand { get; set; }
    public ICommand LoadMedicalRecordCommand { get; set; }
    public ICommand LoadOldAppointmentsCommand { get; set; }
    public ICommand LoadDoctorsCommand { get; set; }


    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public void LoadAppointments()
    {
        CurrentView = new AppointmentTableViewModel(_scheduleService, _doctorService, _patient);
    }

    public void LoadMedicalRecord()
    {
        CurrentView =
            new MedicalRecordViewModel(_medicalRecordService.GetById(_patient.Email), _medicalRecordService);
    }

    public void LoadOldAppointments()
    {
        CurrentView = new OldAppointmentsViewModel(_scheduleService, _doctorService, _patient);
    }

    public void LoadDoctors()
    {
        CurrentView = new SearchDoctorsViewModel(_doctorService,_scheduleService,_patient);
    }

}