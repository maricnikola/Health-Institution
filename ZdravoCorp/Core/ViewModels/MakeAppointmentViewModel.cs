using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.MedicalRecord;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.TimeSlots;

namespace ZdravoCorp.Core.ViewModels;

public class MakeAppointmentViewModel : ViewModelBase
{
    private readonly ObservableCollection<String> _doctors;
    private ScheduleRepository _scheduleRepository;
    private DoctorRepository _doctorRepository;
    private Patient _patient;
    public IEnumerable<String> AllDoctors => _doctors;
    public ICommand CreateAppointmentCommand { get; set; }

    private string _doctorName;

    public string DoctorName
    {
        get { return _doctorName; }
        set
        {
            _doctorName = value;
            OnPropertyChanged(nameof(DoctorName));
        }
    }

    private DateTime _date = DateTime.Now + TimeSpan.FromHours(1);

    public DateTime Date
    {
        get { return _date; }
        set
        {
            _date = value;
            OnPropertyChanged(nameof(Date));
        }
    }

    private int _hours = 00;

    public int Hours
    {
        get { return _hours; }
        set
        {
            _hours = value;
            OnPropertyChanged(nameof(Hours));
        }
    }

    private int _minutes = 00;

    public int Minutes
    {
        get { return _minutes; }
        set
        {
            _minutes = value;
            OnPropertyChanged(nameof(Minutes));
        }
    }


    public MakeAppointmentViewModel(List<Doctor> doctors, ScheduleRepository scheduleRepository,
        ObservableCollection<AppointmentViewModel> Appointments, DoctorRepository doctorRepository, Patient patient)
    {
        _doctorRepository = doctorRepository;
        _scheduleRepository = scheduleRepository;
        _patient = patient;
        _doctors = new ObservableCollection<String>();
        foreach (var doctor in doctors)
        {
            _doctors.Add(doctor.FullName + "-" + doctor.Email);
        }

        CreateAppointmentCommand = new DelegateCommand(o => CreateAppointment(Appointments));
    }

    public void CreateAppointment(ObservableCollection<AppointmentViewModel> Appointments)
    {
        try
        {
            int h = Hours;
            int m = Minutes;
            DateTime d = Date;
            String dm = DoctorName;

            DateTime start = new DateTime(d.Year, d.Month, d.Day, h, m, 0);
            DateTime end = start.AddMinutes(15);
            TimeSlot time = new TimeSlot(start, end);

            string[] tokens = dm.Split("-");
            string mail = tokens[1];
            Doctor doctor = _doctorRepository.GetDoctorByEmail(mail);

            MedicalRecord medicalRecord = new MedicalRecord(_patient);

            Appointment appointment = _scheduleRepository.CreateAppointment(time, doctor, medicalRecord);
            if (appointment != null)
                Appointments.Add(new AppointmentViewModel(appointment));
            else
            {
                MessageBox.Show("Invalid Appointment", "Error", MessageBoxButton.OK);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("Invalid Appointment", "Error", MessageBoxButton.OK);
        }
    }
}