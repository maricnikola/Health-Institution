using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.ViewModels.DirectorViewModel;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Commands;
using System.Windows;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class DEquipmentSpentViewModel :ViewModelBase
{
    private InventoryRepository _inventoryRepository;
    private ObservableCollection<DynamicInventoryViewModel> _dynamicInventory;
    private int _roomId;
    public IEnumerable<DynamicInventoryViewModel> DynamicInventory

    {
        get { return _dynamicInventory; }
        set
        {
            _dynamicInventory = new ObservableCollection<DynamicInventoryViewModel>(value);
            OnPropertyChanged();
        }
    }
    public ICommand ConfirmSpentQuantity { get; }
    public DEquipmentSpentViewModel(InventoryRepository inventoryRepository,int roomId)
    {
        _roomId = roomId;
        _dynamicInventory = new ObservableCollection<DynamicInventoryViewModel>();
        _inventoryRepository = inventoryRepository;
        foreach (var inventoryItem in _inventoryRepository.GetDynamic())
        { 
            if(inventoryItem.RoomId == _roomId)
            {
                _dynamicInventory.Add(new DynamicInventoryViewModel(inventoryItem));
            }
        }
        ConfirmSpentQuantity = new DelegateCommand(o => ConfirmChanges());

    }

    private void CloseWindow()
    {
        Window activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }
    public void ConfirmChanges()
    {
        bool correctnessCheck = true;
        foreach(var inventoryItem in _dynamicInventory)
        {
            if (!inventoryItem.IsChecked) continue;
            if (inventoryItem.OrderQuantity <= 0 || inventoryItem.OrderQuantity > inventoryItem.Quantity)
            {
                correctnessCheck = false;
                break;
            }
            else
            {
                _inventoryRepository.GetInventoryById(inventoryItem.Id).Quantity -= inventoryItem.OrderQuantity;
                Serializer.Save(_inventoryRepository);
            }
        }
        if (correctnessCheck) CloseWindow();
        else MessageBox.Show("Invalid data", "Error", MessageBoxButton.OK);
    }
}
