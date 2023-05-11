using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.MedicalRecord;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.MedicalRecord;
using ZdravoCorp.Core.Repositories.Room;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.View.DoctorView;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class PerformAppointmentViewModel: ViewModelBase
{
    private AppointmentViewModel _appointmentViewModel;
    private RoomRepository _roomRepository;
    private Patient _patient;
    private ScheduleRepository _schedulerRepository;
    private PatientRepository _patientRepository;
    private MedicalRecordRepository _medicalRecordRepository;
    private Appointment _appointment;
    private InventoryRepository _inventoryRepository;
    private int _roomId;
    public String PatientMail => _patient.Email ;
    public String PatientName => _patient.FullName;
    public ICommand PerformCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand MedicalRCommand { get; }


    public PerformAppointmentViewModel(Appointment performingAppointment,ScheduleRepository scheduleRepository,PatientRepository patientRepository,MedicalRecordRepository medicalRecordRepository,InventoryRepository inventoryRepository,RoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
        _inventoryRepository = inventoryRepository;
        _appointment = performingAppointment;
        _medicalRecordRepository = medicalRecordRepository;
        _schedulerRepository = scheduleRepository;
        _patientRepository = patientRepository;
        _patient = patientRepository.GetPatientByEmail(performingAppointment.PatientEmail);
        assignRoom();
        
        CancelCommand = new DelegateCommand(o => CloseWindow());
        MedicalRCommand = new DelegateCommand(o => ShowMedicalRecordDialog());
        PerformCommand = new DelegateCommand(o => SavePerformingAppointment());
    }


    private string _symptoms;
    public string Symptoms
    {
        get
        {
            return _symptoms;
        }
        set
        {
            _symptoms = value;
            OnPropertyChanged(nameof(Symptoms));
        }
    }

    private string _opinion;
    public string Opinion
    {
        get
        {
            return _opinion;
        }
        set
        {
            _opinion = value;
            OnPropertyChanged(nameof(Opinion));
        }
    }

    private string _allergens;
    public string Allergens
    {
        get
        {
            return _allergens;
        }
        set
        {
            _allergens = value;
            OnPropertyChanged(nameof(Allergens));
        }
    }

    private string _keyWord;
    public string KeyWord
    {
        get
        {
            return _keyWord;
        }
        set
        {
            _keyWord = value;
            OnPropertyChanged(nameof(KeyWord));
        }
    }
    private void assignRoom()
    {
        
        foreach (Room room in _roomRepository.Rooms)
        {
            bool checkRoom = true;
            foreach (Appointment appointment in _schedulerRepository.GetAllAppointments())
            {
                if(room.Id == appointment.Room && _appointment.Time.Overlap(appointment.Time))
                {
                    checkRoom = false;
                }
            }
            if (!checkRoom) continue;
            _roomId = room.Id;
            return;
        }
    }
    private void CloseWindow()
    {
        Window activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }

    public void ShowMedicalRecordDialog()
    {
        if (_patient != null)
        {
            MedicalRecord medicalR = _medicalRecordRepository.GetById(_patient.Email);
            ChangeMedicalRecordView window = new ChangeMedicalRecordView() { DataContext = new MedicalRecordViewModel(medicalR, _medicalRecordRepository) };
            window.Show();
        }
        else
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
        }

    }
    public void ShowDEquipmentSpentDialog()
    {
        var window = new DEquipmentSpentView() { DataContext = new DEquipmentSpentViewModel(_inventoryRepository,_roomId) };
        window.Show();
    }


    public void SavePerformingAppointment()
    {
        try
        {
            List<String> patientSymptoms = Symptoms.Trim().Split(",").ToList();
            List<String> patientAllergens = Allergens.Trim().Split(",").ToList();
            String doctorOpinion = Opinion.Trim();
            String anamnesisKeyWord = KeyWord;
            if(_schedulerRepository.CheckPerformingAppointmentData(patientSymptoms, doctorOpinion, patientAllergens,anamnesisKeyWord))
            {
                CloseWindow();
                ShowDEquipmentSpentDialog();
                _schedulerRepository.ChangePerformingAppointment(_appointment.Id, patientSymptoms, doctorOpinion, patientAllergens,anamnesisKeyWord,_roomId);
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
}

