using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services.Services;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalAssets.Equipment.ViewModels;

public class DynamicEquipmentSpentViewModel : ViewModelBase
{
    private ObservableCollection<DynamicInventoryViewModel> _dynamicInventory;
    private readonly IInventoryService _inventoryService;
    private readonly IRoomService _roomService;
    private readonly int _roomId;

    public DynamicEquipmentSpentViewModel(IInventoryService inventoryService,IRoomService roomService, int roomId)
    {
        _roomService = roomService;
        _roomId = roomId;
        _dynamicInventory = new ObservableCollection<DynamicInventoryViewModel>();
        _inventoryService = inventoryService;
        foreach (var inventoryItem in _inventoryService.GetDynamic())
            if (inventoryItem.RoomId == _roomId)
                _dynamicInventory.Add(new DynamicInventoryViewModel(inventoryItem));
        ConfirmSpentQuantity = new DelegateCommand(o => ConfirmChanges());
    }

    public IEnumerable<DynamicInventoryViewModel> DynamicInventory

    {
        get => _dynamicInventory;
        set
        {
            _dynamicInventory = new ObservableCollection<DynamicInventoryViewModel>(value);
            OnPropertyChanged();
        }
    }

    public ICommand ConfirmSpentQuantity { get; }

    private void CloseWindow()
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }

    public void ConfirmChanges()
    {
        var correctnessCheck = true;
        foreach (var inventoryItem in _dynamicInventory)
        {
            if (!inventoryItem.IsChecked) continue;
            if (inventoryItem.OrderQuantity <= 0 || inventoryItem.OrderQuantity > inventoryItem.Quantity)
            {
                correctnessCheck = false;
                break;
            }
            _inventoryService.UpdateSpentInventory(inventoryItem.Id, inventoryItem.OrderQuantity);
        }

        if (correctnessCheck) CloseWindow();
        else MessageBox.Show("Invalid data", "Error", MessageBoxButton.OK);
    }
}