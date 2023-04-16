namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class PatientViewModel : ViewModelBase
{
    private object _currentView;
    
    

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
    
    public PatientViewModel()
    {
        _currentView = new AppointmentTableViewModel();
    }
}