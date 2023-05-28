using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.Services.PatientServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Services.MedicalRecordServices;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Services.DoctorServices;
using ZdravoCorp.Core.Services.HospitalRefferalServices;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Services.RoomServices;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class AddAppointmentViewModel : ViewModelBase
{
    private readonly DateTime _date;
    private readonly Doctor _dr;
    private readonly IMedicalRecordService _medicalRecordService;
    private readonly IPatientService _patientService;
    private readonly IScheduleService _scheduleService;
    private readonly IHospitalRefferalService _hospitalRefferalService;
    private readonly IRoomService _roomService;
    private int _roomId;

    private DateTime _startDate = DateTime.Now + TimeSpan.FromHours(1);

    private int _startTimeHours;
    private int _startTimeMinutes;


    private string _username;


    public AddAppointmentViewModel(IScheduleService scheduleService, IDoctorService doctorService,
        ObservableCollection<AppointmentViewModel> appointment, IPatientService patientService, Doctor doctor,
        IMedicalRecordService medicalRecordService, DateTime date,IHospitalRefferalService hospitalRefferalService,IRoomService roomService)
    {
        _roomService = roomService;
        _dr = doctor;
        _medicalRecordService = medicalRecordService;
        _date = date;
        PossibleMinutes = new[] { 00, 15, 30, 45 };
        PossibleHours = new[]
            { 00, 01, 02, 03, 04, 05, 06, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };

        _hospitalRefferalService = hospitalRefferalService;
        _scheduleService = scheduleService;
        _patientService = patientService;
        var _controller = new PatientRepository();
        var patients = _controller.Patients;

        _patientsFullname = new ObservableCollection<string>();
        foreach (var p in patients) _patientsFullname.Add(p.FullName + "-" + p.Email);

        AddCommand = new DelegateCommand(o => DrCreateAppointment(appointment, _date));
        CancelCommand = new DelegateCommand(o => CloseWindow());
    }

    private ObservableCollection<string> _patientsFullname { get; }
    public IEnumerable<string> Patients => _patientsFullname;

    public int[] PossibleMinutes { get; set; }
    public int[] PossibleHours { get; set; }

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }

    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            _startDate = value;
            OnPropertyChanged();
        }
    }

    public int StartTimeHours
    {
        get => _startTimeHours;
        set
        {
            _startTimeHours = value;
            OnPropertyChanged();
        }
    }

    public int StartTimeMinutes
    {
        get => _startTimeMinutes;
        set
        {
            _startTimeMinutes = value;
            OnPropertyChanged();
        }
    }


    public ICommand AddCommand { get; }
    public ICommand CancelCommand { get; }
    private void AssignRoom(TimeSlot time)
    {
        foreach (var room in _roomService.GetAll())
        {
            if (room.IsUnderRenovation || room.Type != Models.Rooms.RoomType.ExaminationRoom
                || !_scheduleService.CheckRoomAvailability(room.Id,time)) continue;
            _roomId = room.Id;
            return;
        }
    }
    private void CloseWindow()
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }

    public void DrCreateAppointment(ObservableCollection<AppointmentViewModel> Appointments, DateTime date)
    {
        try
        {
            var hours = StartTimeHours;
            var minutes = StartTimeMinutes;
            var d = StartDate;
            var dm = Username;

            var start = new DateTime(d.Year, d.Month, d.Day, hours, minutes, 0);
            var end = start.AddMinutes(15);
            var time = new TimeSlot(start, end);

            var tokens = dm.Split("-");
            var mail = tokens[1];
            var patient = _patientService.GetByEmail(mail);

            var patientInHospital = _hospitalRefferalService.IsPatientOnHospitalTreatment(patient.Email,time);
            if (patientInHospital)
            {
                MessageBox.Show("Patient is on hospital treatment", "Error", MessageBoxButton.OK);
                return;
            }
            AssignRoom(time);
            var appointment = _scheduleService.CreateAppointment(time, _dr, mail,_roomId);

            if (appointment != null)
            {
                
                CloseWindow();
                if (_scheduleService.IsForShow(appointment, date))
                    Appointments.Add(new AppointmentViewModel(appointment));
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