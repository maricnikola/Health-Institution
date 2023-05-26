using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Services.PatientServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Services.MedicalRecordServices;
using ZdravoCorp.View.DoctorView;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.RoomServices;
using ZdravoCorp.Core.Services.SpecialistsRefferalServices;
using ZdravoCorp.Core.Utilities;
using Autofac;
using ZdravoCorp.Core.Services.DoctorServices;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class PerformAppointmentViewModel : ViewModelBase
{
    private string _allergens;
    private readonly Appointment _appointment;
    private AppointmentViewModel _appointmentViewModel;
    private readonly IInventoryService _inventoryService;

    private string _keyWord;
    private ISpecialistsRefferalService _specialistsRefferalService;
    private readonly IMedicalRecordService _medicalRecordService;

    private string _opinion;
    private readonly Patient? _patient;
    private IPatientService _patientService;
    private int _roomId;
    private readonly IRoomService _roomService;
    private readonly IScheduleService _scheduleService;
    private readonly IDoctorService _doctorService;


    private string _symptoms;


    public PerformAppointmentViewModel(Appointment performingAppointment, IScheduleService scheduleService,
        IPatientService patientService, IMedicalRecordService medicalRecordService,
        IInventoryService inventoryService, IRoomService roomService,IDoctorService doctorService)
    {
        _doctorService = doctorService;
        _roomService = roomService;
        _specialistsRefferalService = Injector.Container.Resolve<ISpecialistsRefferalService>();
        _inventoryService = inventoryService;
        _appointment = performingAppointment;
        _medicalRecordService = medicalRecordService;
        _scheduleService = scheduleService;
        _patientService = patientService;
        _patient = _patientService.GetByEmail(performingAppointment.PatientEmail);
        assignRoom();

        CancelCommand = new DelegateCommand(o => CloseWindow());
        MedicalRCommand = new DelegateCommand(o => ShowMedicalRecordDialog());
        PerformCommand = new DelegateCommand(o => SavePerformingAppointment());
        EntryRefferal = new DelegateCommand(o => ShowEntryRefferal());
    }

    public string PatientMail => _patient.Email;
    public string PatientName => _patient.FullName;
    public ICommand PerformCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand MedicalRCommand { get; }
    public ICommand EntryRefferal { get; }

    public string Symptoms
    {
        get => _symptoms;
        set
        {
            _symptoms = value;
            OnPropertyChanged();
        }
    }

    public string Opinion
    {
        get => _opinion;
        set
        {
            _opinion = value;
            OnPropertyChanged();
        }
    }

    public string Allergens
    {
        get => _allergens;
        set
        {
            _allergens = value;
            OnPropertyChanged();
        }
    }

    public string KeyWord
    {
        get => _keyWord;
        set
        {
            _keyWord = value;
            OnPropertyChanged();
        }
    }

    private void assignRoom()
    {
        foreach (var room in _roomService.GetAll())
        {
            var checkRoom = true;
            foreach (var appointment in _scheduleService.GetAllAppointments())
                if (room.Id == appointment.Room && _appointment.Time.Overlap(appointment.Time))
                    checkRoom = false;
            if (!checkRoom) continue;
            _roomId = room.Id;
            return;
        }
    }

    private void CloseWindow()
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }

    public void ShowMedicalRecordDialog()
    {
        if (_patient != null)
        {
            var medicalR = _medicalRecordService.GetById(_patient.Email);
            var window = new ChangeMedicalRecordView
                { DataContext = new MedicalRecordViewModel(medicalR, _medicalRecordService) };
            window.Show();
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }
    }

    public void ShowDEquipmentSpentDialog()
    {
        var window = new DEquipmentSpentView
            { DataContext = new DEquipmentSpentViewModel(_inventoryService, _roomService,_roomId) };
        window.Show();
    }


    public void SavePerformingAppointment()
    {
        try
        {
            var patientSymptoms = Symptoms.Trim().Split(",").ToList();
            var patientAllergens = Allergens.Trim().Split(",").ToList();
            var doctorOpinion = Opinion.Trim();
            var anamnesisKeyWord = KeyWord;
            if (_scheduleService.CheckPerformingAppointmentData(patientSymptoms, doctorOpinion, patientAllergens,
                    anamnesisKeyWord))
            {
                CloseWindow();
                ShowDEquipmentSpentDialog();
                _scheduleService.ChangePerformingAppointment(_appointment.Id, patientSymptoms, doctorOpinion,
                    patientAllergens, anamnesisKeyWord, _roomId);
            }
            else
            {
                MessageBox.Show("Invalid data for performing", "Error", MessageBoxButton.OK);
            }
        }
        catch (Exception)
        {
            MessageBox.Show("Invalid data for performing", "Error", MessageBoxButton.OK);
        }
    }

    public void ShowEntryRefferal()
    {
        CloseWindow();
        var window = new AddSpecialistsRefferalView() { DataContext = new AddSpecialistsRefferalViewModel(this,_doctorService,_appointment.Doctor,_patient,_scheduleService, _appointment)};
        window.Show();
    }
}