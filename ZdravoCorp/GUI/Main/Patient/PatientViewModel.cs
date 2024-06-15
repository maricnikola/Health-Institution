using System.Windows.Input;
using Autofac;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services;
using ZdravoCorp.Core.HospitalSystem.Analytics.Services;
using ZdravoCorp.Core.HospitalSystem.Notifications.Services;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.PatientFiles.MedicalRecords.Services;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.CronJobs;
using ZdravoCorp.GUI.HospitalSystem.Analytics.ViewModels;
using ZdravoCorp.GUI.HospitalSystem.Notifications.ViewModels;
using ZdravoCorp.GUI.HospitalSystem.Users.ViewModels;
using ZdravoCorp.GUI.PatientFiles.MedicalRecords.ViewModels;
using ZdravoCorp.GUI.Scheduling.ViewModels;

namespace ZdravoCorp.GUI.Main.Patient;

public class PatientViewModel : ViewModelBase
{
    private object _currentView;
    private readonly IDoctorService _doctorService;
    private readonly IMedicalRecordService _medicalRecordService;
    private readonly IPatientService _patientService;
    private readonly INotificationService _notificationService;
    private readonly ISurveyService _surveyService;

    private readonly Core.HospitalSystem.Users.Models.Patient _patient;
    private readonly IScheduleService _scheduleService;
    private IRoomService _roomService;

    public PatientViewModel(User user)
    {
        _doctorService = Injector.Container.Resolve<IDoctorService>();
        _medicalRecordService = Injector.Container.Resolve<IMedicalRecordService>();
        _scheduleService = Injector.Container.Resolve<IScheduleService>();
        _patientService = Injector.Container.Resolve<IPatientService>();
        _roomService = Injector.Container.Resolve<IRoomService>();
        _notificationService = Injector.Container.Resolve<INotificationService>();
        _surveyService = Injector.Container.Resolve<ISurveyService>();

        _patient = _patientService.GetByEmail(user.Email);
        LoadAppointmentsCommand = new DelegateCommand(o => LoadAppointments());
        LoadMedicalRecordCommand = new DelegateCommand(o => LoadMedicalRecord());
        LoadOldAppointmentsCommand = new DelegateCommand(o => LoadOldAppointments());
        LoadDoctorsCommand = new DelegateCommand(o => LoadDoctors());
        LoadNotificationsCommand = new DelegateCommand(o => LoadNotifications());
        LoadHospitalSurvayCommand = new DelegateCommand(o => LoadHospitalSurvey());

        _currentView = new AppointmentTableViewModel(_scheduleService, _doctorService, _patient,_roomService);
        JobScheduler.LoadUsersNotifications(user.Email);

    }

    public ICommand LoadAppointmentsCommand { get; set; }
    public ICommand LoadMedicalRecordCommand { get; set; }
    public ICommand LoadOldAppointmentsCommand { get; set; }
    public ICommand LoadDoctorsCommand { get; set; }
    public ICommand LoadNotificationsCommand { get; set; }
    public ICommand LoadHospitalSurvayCommand { get; set; }

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
        CurrentView = new AppointmentTableViewModel(_scheduleService, _doctorService, _patient,_roomService);
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
        CurrentView = new SearchDoctorsViewModel(_doctorService,_scheduleService,_patient,_roomService);
    }

    private void LoadNotifications()
    {
        CurrentView = new AllNotificationsViewModel(_notificationService,_patientService,_patient.Email);
    }

    private void LoadHospitalSurvey()
    {
        CurrentView = new CreateHospitalSurveyViewModel(_surveyService, _patient.Email);
    }


}