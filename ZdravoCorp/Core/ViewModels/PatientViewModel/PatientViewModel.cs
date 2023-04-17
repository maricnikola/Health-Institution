using System.Collections.Generic;
using System.Windows.Documents;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class PatientViewModel : ViewModelBase
{
    private object _currentView;
    private List<Appointment> _appointments;
    private ScheduleRepository _scheduleRepository;
    private DoctorRepository _doctorRepository;
    private Patient _patient;
    
    

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
    
    public PatientViewModel(List<Appointment> appointments, ScheduleRepository scheduleRepository, DoctorRepository doctorRepository, Patient patient)
    {
        _appointments=appointments;
        _scheduleRepository = scheduleRepository;
        _doctorRepository = doctorRepository;
        _patient = patient;
        _currentView = new AppointmentTableViewModel(_appointments, scheduleRepository, _doctorRepository, _patient);
    }
}