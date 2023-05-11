using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.MedicalRecord;
using ZdravoCorp.Core.Repositories.Room;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.ViewModels.DirectorViewModel;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class DoctorViewModel : ViewModelBase
{
    private InventoryRepository _inventoryRepository;
    private object _currentView;
    private User _user;
    private ScheduleRepository _scheduleRepository;
    private DoctorRepository _doctorRepository;
    private PatientRepository _patientRepository;
    private MedicalRecordRepository _medicalRecordRepository;
    private RoomRepository _roomRepository;
    private Doctor _doctor;

    public ICommand LoadAppointmentCommand { get; private set; }
    public ICommand LoadPatientsCommand { get; private set; }


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

    public DoctorViewModel(User user, RepositoryManager repositoryManager)
    {
        _inventoryRepository = repositoryManager.InventoryRepository;
        _roomRepository = repositoryManager.RoomRepository;
        _doctorRepository = repositoryManager.DoctorRepository;
        _scheduleRepository = repositoryManager.ScheduleRepository;
        _doctor = _doctorRepository.GetDoctorByEmail(user.Email);
        _patientRepository = repositoryManager.PatientRepository;
        List<Appointment> appointments = _scheduleRepository.GetDoctorAppointments(_doctor.Email);
        _medicalRecordRepository = repositoryManager.MedicalRecordRepository;

        _user = user;
        LoadAppointmentCommand = new DelegateCommand(o => LoadAppointments());
        LoadPatientsCommand = new DelegateCommand(o => LoadPatinets());
        _currentView = new AppointmentShowViewModel(_user, _scheduleRepository, _doctorRepository, _patientRepository,_medicalRecordRepository,_inventoryRepository,_roomRepository);
    }

    public void LoadAppointments()
    {
        CurrentView = new AppointmentShowViewModel(_user, _scheduleRepository, _doctorRepository, _patientRepository,_medicalRecordRepository,_inventoryRepository,_roomRepository);
    }

    public void LoadPatinets()
    {
        CurrentView = new PatientTableViewModel(_user,_scheduleRepository, _doctorRepository, _patientRepository,_medicalRecordRepository);
    }
}
