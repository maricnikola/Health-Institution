using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;

namespace ZdravoCorp.Core.ViewModels;

public class MakeAppointmentViewModel : ViewModelBase
{
    private readonly ObservableCollection<String> _doctors;
    public IEnumerable<String> AllDoctors => _doctors;
    public ICommand CreateAppointmentCommand { get; set; }

    private string _doctorName;
    public string DoctorName
    {
        get
        {
            return _doctorName;
        }
        set
        {
            _doctorName = value;
            OnPropertyChanged(nameof(DoctorName));
        }
    }
    private DateTime _date;
    public DateTime Date
    {
        get
        {
            return _date;
        }
        set
        {
            _date = value;
            OnPropertyChanged(nameof(Date));
        }
    }
    private int _hours;
    public int Hours
    {
        get
        {
            return _hours;
        }
        set
        {
            _hours = value;
            OnPropertyChanged(nameof(Hours));
        }
    }
    private int _minutes;
    public int Minutes
    {
        get
        {
            return _minutes;
        }
        set
        {
            _minutes = value;
            OnPropertyChanged(nameof(Minutes));
        }
    }


    public MakeAppointmentViewModel(List<Doctor> doctors)
    {
        _doctors = new ObservableCollection<String>();
        foreach (var doctor in doctors)
        {
            _doctors.Add(doctor.FullName);
        }

        CreateAppointmentCommand = new DelegateCommand(o => CreateAppointment());

    }

    public void CreateAppointment()
    {
        int h = Hours;
        int m = Minutes;
        DateTime d = Date;
        String dm = DoctorName;
    }

}