using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Inventory;
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

    public DoctorViewModel(User user, ScheduleRepository scheduleRepository, DoctorRepository doctorRepository, PatientRepository patientRepository)
    {
        _user = user;
        _scheduleRepository = scheduleRepository;
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
        LoadAppointmentCommand = new DelegateCommand(o => LoadAppointments());
        LoadPatientsCommand = new DelegateCommand(o => LoadPatinets());
        _currentView = new AppointmentShowViewModel(_user, _scheduleRepository, _doctorRepository, _patientRepository);
    }

    public void LoadAppointments()
    {
        CurrentView = new AppointmentShowViewModel(_user, _scheduleRepository, _doctorRepository, _patientRepository);
    }

    public void LoadPatinets()
    {
        CurrentView = new PatientTableViewModel(_user, _doctorRepository, _patientRepository);
    }
}
