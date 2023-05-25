using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.RoomServices;
using ZdravoCorp.Core.Services.ScheduleServices;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class CreatePrescriptionsViewModel : ViewModelBase
{
    public ObservableCollection<PrescriptionViewModel> Presriptions { get; }

    private IScheduleService _scheduleService;
    private Appointment _appointment;
    private IInventoryService _inventoryService;
    private IRoomService _roomService;
    private int _roomId;
    public CreatePrescriptionsViewModel(Appointment appointment,IScheduleService scheduleService,
        IInventoryService inventoryService,IRoomService roomService,int roomId)
    {
        PossibleTimes = new[] { 1, 2, 3, 4, 5, 6 };
        PossibleInstructions = new[] { "beforeMeal", "afterMeal", "duringMeal", "notImportant" };
        PossibleMedicaments = new[] { "paracetamol", "probiotik", "antibiotik", "brufen" };
        _inventoryService = inventoryService;
        _roomService = roomService;
        _roomId = roomId;
        _appointment = appointment;
        _scheduleService = scheduleService;
    }

    public ICommand Save { get; }
    public ICommand Add { get; }
    public ICommand Delete { get; }

    public int[] PossibleTimes { get; }
    public string[] PossibleInstructions { get; }
    public string[] PossibleMedicaments { get; }
}
