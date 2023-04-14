using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.MedicalRecord;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.TimeSlots;

namespace ZdravoCorp.Core.ViewModels;

public class AppointmentShowViewModel: ViewModelBase
{
    private readonly ObservableCollection<AppointmentViewModel> _appointments;

    public IEnumerable<AppointmentViewModel> Appointments => _appointments;
    public ICommand ChangeAppointmentCommand { get; }
    public ICommand CancelAppointmentCommand { get; }
    public ICommand SearchAppointmentCommand { get; }

    AppointmentShowViewModel()
    {

        string datumString = "2023-04-14";
        DateTime datum;

        // Koristeći metodu Parse klase DateTime
        datum = DateTime.Parse(datumString);
        DateTime datum2;
        datum2 = DateTime.Parse(datumString);
        _appointments = new ObservableCollection<AppointmentViewModel>();
        _appointments.Add(new AppointmentViewModel(new Appointment(123, new TimeSlot(datum, datum2), new Doctor("afa@gmail.com", "Nikola", "Maric", Doctor.SpecializationType.Psychologist), new MedicalRecord(new Patient("jasdfj@afa", "Aleksa", "perovic"), 123, 122))));

    }



}
