using System.Threading;
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
using ZdravoCorp.Core.Services.NotificationServices;
using ZdravoCorp.Core.Services.PatientServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.CronJobs;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class PatientViewModel : ViewModelBase
{
    private object _currentView;
    private readonly IDoctorService _doctorService;
    private readonly IMedicalRecordService _medicalRecordService;
    private readonly IPatientService _patientService;
    private readonly INotificationService _notificationService;

    private readonly Patient _patient;
    private readonly IScheduleService _scheduleService;

    public PatientViewModel(User user)
    {
        _doctorService = Injector.Container.Resolve<IDoctorService>();
        _medicalRecordService = Injector.Container.Resolve<IMedicalRecordService>();
        _scheduleService = Injector.Container.Resolve<IScheduleService>();
        _patientService = Injector.Container.Resolve<IPatientService>();
        _notificationService = Injector.Container.Resolve<INotificationService>();

        _patient = _patientService.GetByEmail(user.Email);
        LoadAppointmentsCommand = new DelegateCommand(o => LoadAppointments());
        LoadMedicalRecordCommand = new DelegateCommand(o => LoadMedicalRecord());
        LoadOldAppointmentsCommand = new DelegateCommand(o => LoadOldAppointments());
        LoadDoctorsCommand = new DelegateCommand(o => LoadDoctors());
        LoadNotificationsCommand = new DelegateCommand(o => LoadNotifications());
        _currentView = new AppointmentTableViewModel(_scheduleService, _doctorService, _patient);
        JobScheduler.LoadUsersNotifications(user.Email);

    }

    public ICommand LoadAppointmentsCommand { get; set; }
    public ICommand LoadMedicalRecordCommand { get; set; }
    public ICommand LoadOldAppointmentsCommand { get; set; }
    public ICommand LoadDoctorsCommand { get; set; }
    public ICommand LoadNotificationsCommand { get; set; }


    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    private void LoadAppointments()
    {
        CurrentView = new AppointmentTableViewModel(_scheduleService, _doctorService, _patient);
    }

    private void LoadMedicalRecord()
    {
        CurrentView =
            new MedicalRecordViewModel(_medicalRecordService.GetById(_patient.Email), _medicalRecordService);
    }

    private void LoadOldAppointments()
    {
        CurrentView = new OldAppointmentsViewModel(_scheduleService, _doctorService, _patient);
    }

    private void LoadDoctors()
    {
        CurrentView = new SearchDoctorsViewModel(_doctorService,_scheduleService,_patient);
    }

    private void LoadNotifications()
    {
        CurrentView = new AllNotificationsViewModel(_notificationService,_patientService,_patient.Email);
    }

}