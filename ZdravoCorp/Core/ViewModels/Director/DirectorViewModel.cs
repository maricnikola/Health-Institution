using System.Windows.Input;
using ZdravoCorp.Core.Commands;

namespace ZdravoCorp.Core.ViewModels;

public class DirectorViewModel : ViewModelBase

{
    private object _currentView;

    public ICommand LoadEquipmentCommand { get; private set; }
    public ICommand LoadDynamicEquipmentCommand { get; private set; }
    
    
    public object CurrentView
    {
        get
        {
            return _currentView;
        }
        set
        {
            _currentView = value;
            OnPropertyChanged("CurrentView");
        }
    }

    public DirectorViewModel()
    {
        LoadEquipmentCommand = new DelegateCommand(o => LoadEquipment());
        LoadDynamicEquipmentCommand = new DelegateCommand(o => LoadDynamicEquipment());
        _currentView = new EquipmentViewModel();
    }

    public void LoadEquipment()
    {
        CurrentView = new EquipmentViewModel();
    }

    public void LoadDynamicEquipment()
    {
        CurrentView = new DynamicEquipmentViewModel();
    }
}