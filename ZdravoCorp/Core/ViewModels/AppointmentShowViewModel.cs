using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Loader;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.MedicalRecord;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.TimeSlots;
using ZdravoCorp.View.DoctorView;

namespace ZdravoCorp.Core.ViewModels;

public class AppointmentShowViewModel: ViewModelBase
{
    private readonly ObservableCollection<AppointmentViewModel> _appointments;

    public IEnumerable<AppointmentViewModel> Appointments => _appointments;
    public ICommand ChangeAppointmentCommand { get; }
    public ICommand AddAppointmentCommand { get; }
    public ICommand CancelAppointmentCommand { get; }
    public ICommand SearchAppointmentCommand { get; }
    public ICommand ViewMedicalRecordCommand { get; }

    public AppointmentShowViewModel(User user)
    {

        ScheduleRepository _controller = new ScheduleRepository();

        DoctorRepository doctoRepository = new DoctorRepository();
        Doctor doctor = doctoRepository.GetDoctorByEmail(user.Email);
        LoadFunctions.LoadAppointments(_controller);

        List<Appointment> appointments = _controller.GetDoctorAppointments(doctor);

        _appointments = new ObservableCollection<AppointmentViewModel>();
        foreach(Appointment appointment in appointments)
        {
            _appointments.Add(new AppointmentViewModel(appointment));
        }

        AddAppointmentCommand = new DelegateCommand(o => OpenAddDialog());

        
    }

    public void OpenAddDialog()
    {
        var addAp = new AddAppointmentView() { DataContext =new  AddAppointmentViewModel()};
        addAp.Show();
    }



}
