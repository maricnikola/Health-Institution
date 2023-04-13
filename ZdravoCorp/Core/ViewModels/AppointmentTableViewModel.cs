using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Schedule;

namespace ZdravoCorp.Core.ViewModels;

public class AppointmentTableViewModel: ViewModelBase
{
    private readonly ObservableCollection<AppointmentViewModel> _appointments;

    public IEnumerable<AppointmentViewModel> Appointments => _appointments;
    public Appointment SelectedAppointment { get; set; }

    public AppointmentTableViewModel(List<Appointment> appointments)
    {
        _appointments = new ObservableCollection<AppointmentViewModel>();
        foreach (var appointment in appointments)
        {
            _appointments.Add(new AppointmentViewModel(appointment));
        }    
    }
    
    }