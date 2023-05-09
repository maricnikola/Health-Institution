using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.View.DoctorView;

namespace ZdravoCorp.Core.ViewModels;

public class DoctorWindowViewModel : ViewModelBase
{
    public ICommand AppointmentsCommand { get; }
    public ICommand OperationsCommand { get; }
    private User _user;
    private ScheduleRepository _scheduleRepository;
    private DoctorRepository _doctor;
    private PatientRepository _patientRepository;


    public DoctorWindowViewModel(User user, ScheduleRepository scheduleRepository, DoctorRepository doctorRepository, PatientRepository patientRepository)
    {
        _user = user;   
        _scheduleRepository = scheduleRepository;
        _doctor = doctorRepository; 
       _patientRepository = patientRepository;

        AppointmentsCommand = new DelegateCommand(o => ShowAppointmensCrud());
    }

    public void ShowAppointmensCrud()
    {
        var window = new AppointmentsShowView() { DataContext = new AppointmentShowViewModel(_user, _scheduleRepository, _doctor, _patientRepository) };
        window.Show();
    }
}
