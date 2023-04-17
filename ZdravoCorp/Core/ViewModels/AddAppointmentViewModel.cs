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
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.MedicalRecord;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.TimeSlots;
using ZdravoCorp.View;

namespace ZdravoCorp.Core.ViewModels;

public class AddAppointmentViewModel : ViewModelBase
{
    private ObservableCollection<String> _patientsFullname { get; }
    private ScheduleRepository _scheduleRepository;
    public IEnumerable<String> Patients => _patientsFullname;
    private PatientRepository _patientRepository;
    private Doctor _dr;
    private MedicalRecordRepository _medicalRepository;
    private DateTime _date;


    public AddAppointmentViewModel(ScheduleRepository scheduleRepository, DoctorRepository doctorRepository, ObservableCollection<AppointmentViewModel> appointment, PatientRepository patientRepository, Doctor doctor, MedicalRecordRepository medicalRepository, DateTime date)
    {
        _dr = doctor;
        _medicalRepository = medicalRepository;
        _date = date;   

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
    }


    private string _username;

    public string Username
    {
        get
        {
            return _username;
        }
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    private DateTime _startDate = DateTime.Now + TimeSpan.FromHours(1);
    public DateTime StartDate
    {
        get
        {
            return _startDate;
        }
        set
        {
            _startDate = value;
            OnPropertyChanged(nameof(StartDate));
        }
    }

    private int _startTimeHours;
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
    private int _startTimeMinutes;
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


    public void DrCreateAppointment(ObservableCollection<AppointmentViewModel> Appointments,DateTime date)
    {
        try
        {
            int hours = StartTimeHours;
            int minutes = StartTimeMinutes;
            DateTime d = StartDate;
            String dm = Username;

            DateTime start = new DateTime(d.Year, d.Month, d.Day, hours, minutes, 0);
            DateTime end = start.AddMinutes(15);
            TimeSlot time = new TimeSlot(start, end);

            string[] tokens = dm.Split("-");
            string mail = tokens[1];
            Patient patient = _patientRepository.GetPatientByEmail(mail);

            MedicalRecord medicalRecord = new MedicalRecord(patient);

            Appointment appointment = _scheduleRepository.CreateAppointment(time, _dr, medicalRecord);
            if (appointment != null)
            {
                if (_scheduleRepository.IsForShow(appointment, date))
                {
                    Appointments.Add(new AppointmentViewModel(appointment));
                }

                _medicalRepository.AddRecord(appointment.MedicalRecord);
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
