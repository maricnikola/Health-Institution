using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.MedicalRecord;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.TimeSlots;
using ZdravoCorp.View;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class AdvancedMakeAppointmentViewModel : ViewModelBase
{
    private DoctorRepository _doctorRepository;
    private ScheduleRepository _scheduleRepository;

    private readonly ObservableCollection<AppointmentViewModel> _appointments;
    //public ObservableCollection<AppointmentViewModel> Appointments => _appointments;

    public ObservableCollection<AppointmentViewModel> Appointments => _appointments;

    private readonly ObservableCollection<String> _doctors;
    private Patient _patient;

    public IEnumerable<String> AllDoctors => _doctors;
    public int[] PossibleMinutes { get; set; }
    public int[] PossibleHours { get; set; }
    public string[] PriorityOptions { get; set; }

    public ICommand RecommendAppointmentCommand { get; set; }

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

    private DateTime _date = DateTime.Now + TimeSpan.FromHours(1);
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

    private int _startHours = 00;
    public int StartHours
    {
        get
        {
            return _startHours;
        }
        set
        {
            _startHours = value;
            OnPropertyChanged(nameof(StartHours));
        }
    }
    private int _startMinutes = 00;
    public int StartMinutes
    {
        get
        {
            return _startMinutes;
        }
        set
        {
            _startMinutes = value;
            OnPropertyChanged(nameof(StartMinutes));
        }
    }
    private int _endHours = 00;
    public int EndHours
    {
        get
        {
            return _endHours;
        }
        set
        {
            _endHours = value;
            OnPropertyChanged(nameof(EndHours));
        }
    }
    private int _endMinutes = 00;
    public int EndMinutes
    {
        get
        {
            return _endMinutes;
        }
        set
        {
            _endMinutes = value;
            OnPropertyChanged(nameof(EndMinutes));
        }
    }

    private string _priority;

    public string Priority
    {
        get
        {
            return _priority;
        }
        set
        {
            _priority = value;
            OnPropertyChanged(nameof(Priority));
        }
    }



    public AdvancedMakeAppointmentViewModel(DoctorRepository doctorRepository, ScheduleRepository scheduleRepository, Patient patient)
    {
        _scheduleRepository = scheduleRepository;
        _doctorRepository = doctorRepository;
        _doctors = new ObservableCollection<String>();
        _patient =  patient;
        _appointments = new ObservableCollection<AppointmentViewModel>();
        PossibleMinutes = new[] { 00, 15, 30, 45 };
        PossibleHours = new[]
            { 00, 01, 02, 03, 04, 05, 06, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        PriorityOptions = new[] { "Doctor", "Time" };
        List<Doctor> doctors = doctorRepository.GetAll();
        foreach (var doctor in doctors)
        {
            _doctors.Add(doctor.FullName + "-" + doctor.Email);
        }

        RecommendAppointmentCommand = new DelegateCommand(o=>RecommendAppointments());
    }

    private void RecommendAppointments()
    {
        try
        {
            Appointments.Clear();
            String doc = DoctorName;
            DateTime lastDate = Date;
            lastDate = new DateTime(lastDate.Year, lastDate.Month, lastDate.Day, 23, 59, 0);
            DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, StartHours, StartMinutes, 0);
            DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, EndHours, EndMinutes, 0);
            TimeSlot wantedTimeSlot = new TimeSlot(startTime, endTime);

            string[] tokens = doc.Split("-");
            string doctorsMail = tokens[1];
            Doctor? doctor = _doctorRepository.GetDoctorByEmail(doctorsMail);
            MedicalRecord medicalRecord = new MedicalRecord(_patient);

            string priority = Priority;

            if (Priority.Equals("Doctor"))
            {
                List<Appointment> possibleAppointments = _scheduleRepository.FindAppointmentsByDoctorPriority(doctor, wantedTimeSlot, lastDate, medicalRecord);
                foreach (Appointment singleAppointment in possibleAppointments)
                {
                    Appointments.Add(new AppointmentViewModel(singleAppointment));
                }
            }
            else
            {
                
            }

            
        }
        catch (Exception e)
        {
            MessageBox.Show("Error", "Error", MessageBoxButton.OK);

        }
    }
}