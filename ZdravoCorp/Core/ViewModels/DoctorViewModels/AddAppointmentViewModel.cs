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
using ZdravoCorp.Core.Repositories.MedicalRecord;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.TimeSlots;
using ZdravoCorp.View;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class AddAppointmentViewModel : ViewModelBase
{
    private ObservableCollection<string> _patientsFullname { get; }
    private ScheduleRepository _scheduleRepository;
    public IEnumerable<string> Patients => _patientsFullname;
    private PatientRepository _patientRepository;
    private Doctor _dr;
    private MedicalRecordRepository _medicalRepository;
    private DateTime _date;

    public int[] PossibleMinutes { get; set; }
    public int[] PossibleHours { get; set; }


    public AddAppointmentViewModel(ScheduleRepository scheduleRepository, DoctorRepository doctorRepository,
        ObservableCollection<AppointmentViewModel> appointment, PatientRepository patientRepository, Doctor doctor,
        MedicalRecordRepository medicalRepository, DateTime date)
    {
        _dr = doctor;
        _medicalRepository = medicalRepository;
        _date = date;
        PossibleMinutes = new[] { 00, 15, 30, 45 };
        PossibleHours = new[]
            { 00, 01, 02, 03, 04, 05, 06, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };

        _scheduleRepository = scheduleRepository;
        _patientRepository = patientRepository;
        PatientRepository _controller = new PatientRepository();
        List<Patient> patients = _controller.Patients;

        _patientsFullname = new ObservableCollection<string>();
        foreach (Patient p in patients)
        {
            _patientsFullname.Add(p.FullName + "-" + p.Email);
        }

        AddCommand = new DelegateCommand(o => DrCreateAppointment(appointment, _date));
        CancelCommand = new DelegateCommand(o => CloseWindow());
    }


    private string _username;

    public string Username
    {
        get { return _username; }
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    private DateTime _startDate = DateTime.Now + TimeSpan.FromHours(1);

    public DateTime StartDate
    {
        get { return _startDate; }
        set
        {
            _startDate = value;
            OnPropertyChanged(nameof(StartDate));
        }
    }

    private int _startTimeHours = 00;
    public int StartTimeHours
    {
        get
        {
            return _startTimeHours;
        }
        set
        {
            _startTimeHours = value;
            OnPropertyChanged(nameof(StartTimeHours));
        }
    }
    private int _startTimeMinutes = 00;
    public int StartTimeMinutes
    {
        get
        {
            return _startTimeMinutes;
        }
        set
        {
            _startTimeMinutes = value;
            OnPropertyChanged(nameof(StartTimeMinutes));
        }
    }


    public ICommand AddCommand { get; }
    public ICommand CancelCommand { get; }

    private void CloseWindow()
    {
        Window activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }

    public void DrCreateAppointment(ObservableCollection<AppointmentViewModel> Appointments, DateTime date)
    {
        try
        {
            int hours = StartTimeHours;
            int minutes = StartTimeMinutes;
            DateTime d = StartDate;
            string dm = Username;

            DateTime start = new DateTime(d.Year, d.Month, d.Day, hours, minutes, 0);
            DateTime end = start.AddMinutes(15);
            TimeSlot time = new TimeSlot(start, end);

            string[] tokens = dm.Split("-");
            string mail = tokens[1];
            Patient patient = _patientRepository.GetPatientByEmail(mail);

            Appointment appointment = _scheduleRepository.CreateAppointment(time, _dr, mail);

            if (appointment != null)
            {
                CloseWindow();
                if (_scheduleRepository.IsForShow(appointment, date))
                {
                    Appointments.Add(new AppointmentViewModel(appointment));
                }
            }
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