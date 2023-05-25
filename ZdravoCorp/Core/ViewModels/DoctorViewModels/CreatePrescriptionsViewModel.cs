using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.AnamnesisReport;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.Presriptions;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.RoomServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.View.DoctorView;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class CreatePrescriptionsViewModel : ViewModelBase
{
    public ObservableCollection<PrescriptionViewModel> Prescriptions { get; }
    private List<Prescription> _prescriptions { get; }

    public PrescriptionViewModel SelectedPrescription { get; set; }
    private IScheduleService _scheduleService;
    private Appointment _appointment;
    private IInventoryService _inventoryService;
    private IRoomService _roomService;
    private Anamnesis _anamnesis;
    private int _roomId;
    public CreatePrescriptionsViewModel(Appointment appointment,IScheduleService scheduleService,
        IInventoryService inventoryService,IRoomService roomService,int roomId,Anamnesis anamnesis)
    {
        _anamnesis = anamnesis;
        _prescriptions = new List<Prescription>();
        Prescriptions = new ObservableCollection<PrescriptionViewModel>();
        PossibleTimes = new[] { 1, 2, 3, 4, 5, 6 };
        PossibleInstructions = new[] { "beforeMeal", "afterMeal", "duringMeal", "notImportant" };
        PossibleMedicaments = new[] { "paracetamol", "probiotik", "antibiotik", "brufen" };
        _inventoryService = inventoryService;
        _roomService = roomService;
        _roomId = roomId;
        _appointment = appointment;
        _scheduleService = scheduleService;
        Add = new DelegateCommand(o => AddPrescription());
        Delete = new DelegateCommand(o => DeletePrescription());
        Save = new DelegateCommand(o => SavePerformingAppointment());
    }

    public ICommand Save { get; }
    public ICommand Add { get; }
    public ICommand Delete { get; }

    public int[] PossibleTimes { get; }
    public string[] PossibleInstructions { get; }
    public string[] PossibleMedicaments { get; }

    private int _selectedTime;
    public int SelectedTime
    {
        get
        {
            return _selectedTime;
        }
        set
        {
            _selectedTime = value;
            OnPropertyChanged(nameof(SelectedTime));
        }
    }
    private string _selectedInstruction;
    public string SelectedInstruction
    {
        get
        {
            return _selectedInstruction;
        }
        set
        {
            _selectedInstruction = value;
            OnPropertyChanged(nameof(SelectedInstruction));
        }
    }
    private string  _selectedMedicament;
    public string  SelectedMedicament
    {
        get
        {
            return _selectedMedicament;
        }
        set
        {
            _selectedMedicament = value;
            OnPropertyChanged(nameof(SelectedMedicament));
        }
    }
   

    public void AddPrescription()
    {
        if (checkPrescriptionData())
        {
            MessageBox.Show("Invalid data for prescription", "Error", MessageBoxButton.OK);
            return;
        }
        var PrescriptionModel = new PrescriptionViewModel(new Prescription(SelectedMedicament, SelectedTime, SelectedInstruction));
        foreach (var prescription in Prescriptions)
        {
            if (PrescriptionModel.Instructions == prescription.Instructions && PrescriptionModel.Medicament == prescription.Medicament
                && PrescriptionModel.TimesADay == prescription.TimesADay)
            {
                MessageBox.Show("Prescription already exists", "Error", MessageBoxButton.OK);
                return;
            }
        }
        Prescriptions.Add(PrescriptionModel);
  

    }
    private bool checkPrescriptionData()
    {
        return SelectedInstruction == null  || SelectedMedicament == null || SelectedTime == null;
    }
    public void DeletePrescription()
    {
        if(SelectedPrescription == null)
        {
            MessageBox.Show("None selected", "Error", MessageBoxButton.OK);
            return;
        }
        Prescriptions.Remove(SelectedPrescription);
    }

    public void SavePerformingAppointment()
    {
        if(Prescriptions.Count == 0)
        {
            MessageBox.Show("No prescription added", "Error", MessageBoxButton.OK);
            return;
        }
        CloseWindow();
        _scheduleService.ChangePerformingAppointment(_appointment.Id,_anamnesis, _roomId,_prescriptions);
        ShowDEquipmentSpentDialog();
    }
    public void ShowDEquipmentSpentDialog()
    {
        var window = new DEquipmentSpentView
        { DataContext = new DEquipmentSpentViewModel(_inventoryService, _roomService, _roomId) };
        window.Show();
    }
    private void CloseWindow()
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }
}
