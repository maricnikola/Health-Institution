using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Services.HospitalRefferalServices;
using ZdravoCorp.Core.Services.SpecialistsRefferalServices;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.View.DoctorView;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class AddHospitalRefferalViewModel: ViewModelBase
{

    private PerformAppointmentViewModel _performAppointmentViewModel;

    public ICommand Close { get; }
    public ICommand CreateRefferal { get; }
    public IHospitalRefferalService _hospitalRefferalService;
    public AddHospitalRefferalViewModel(PerformAppointmentViewModel performAppointmentViewModel)
    {
        _hospitalRefferalService = Injector.Container.Resolve<IHospitalRefferalService>();
        _performAppointmentViewModel = performAppointmentViewModel;
        Close = new DelegateCommand(o => CloseWindow(true));
    }

    private int _duration;
    public int Duration
    {
        get
        {
            return _duration;
        }
        set
        {
            _duration = value;
            OnPropertyChanged(nameof(Duration));
        }
    }
    private string _initialTherapy;
    public string InitialTherapy
    {
        get
        {
            return _initialTherapy;
        }
        set
        {
            _initialTherapy = value;
            OnPropertyChanged(nameof(InitialTherapy));
        }
    }
    private string _additionTests;
    public string AdditionTests
    {
        get
        {
            return _additionTests;
        }
        set
        {
            _additionTests = value;
            OnPropertyChanged(nameof(AdditionTests));
        }
    }
    private void CloseWindow(bool backToPerform)
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
        if (backToPerform)
        {
            var performWindow = new PerformAppointmentView() { DataContext = _performAppointmentViewModel };
            performWindow.Show();
        }
    }
}
