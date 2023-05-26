using Autofac;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Services.DoctorServices;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.MedicalRecordServices;
using ZdravoCorp.Core.Services.PatientServices;
using ZdravoCorp.Core.Services.RoomServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Services.SpecialistsRefferalServices;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class DoctorViewModel : ViewModelBase
{
    private object _currentView;
    private readonly Doctor _doctor;
    private readonly IDoctorService _doctorService;
    private readonly IInventoryService _inventoryService;
    private readonly IMedicalRecordService _medicalRecordService;
    private readonly IPatientService _patientService;
    private readonly IRoomService _roomService;
    private readonly IScheduleService _scheduleService;
    private readonly ISpecialistsRefferalService _specialistsRefferalService;
    private readonly User _user;

    public DoctorViewModel(User user)
    {
        _doctorService = Injector.Container.Resolve<IDoctorService>();
        _inventoryService = Injector.Container.Resolve<IInventoryService>();
        _medicalRecordService = Injector.Container.Resolve<IMedicalRecordService>();    
        _patientService = Injector.Container.Resolve<IPatientService>();    
        _roomService = Injector.Container.Resolve<IRoomService>();
        _scheduleService = Injector.Container.Resolve<IScheduleService>();
        _specialistsRefferalService = Injector.Container.Resolve<ISpecialistsRefferalService>();
        _doctor = _doctorService.GetByEmail(user.Email);
        
        var appointments = _scheduleService.GetDoctorAppointments(_doctor.Email);
        _user = user;

       

        LoadAppointmentCommand = new DelegateCommand(o => LoadAppointments());
        LoadPatientsCommand = new DelegateCommand(o => LoadPatinets());
        _currentView = new AppointmentShowViewModel(_user, _scheduleService, _doctorService, _patientService,
            _medicalRecordService, _inventoryService, _roomService);
    }

    public ICommand LoadAppointmentCommand { get; private set; }
    public ICommand LoadPatientsCommand { get; private set; }


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
        CurrentView = new AppointmentShowViewModel(_user, _scheduleService, _doctorService, _patientService,
            _medicalRecordService, _inventoryService, _roomService);
    }

    public void LoadPatinets()
    {
        CurrentView = new PatientTableViewModel(_user, _scheduleService, _doctorService, _patientService,
            _medicalRecordService);
    }
}