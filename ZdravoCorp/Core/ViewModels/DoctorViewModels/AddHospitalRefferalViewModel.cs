using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.View.DoctorView;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class AddHospitalRefferalViewModel: ViewModelBase
{

    private PerformAppointmentViewModel _performAppointmentViewModel;

    public ICommand Close { get; }
    public AddHospitalRefferalViewModel(PerformAppointmentViewModel performAppointmentViewModel)
    {
        _performAppointmentViewModel = performAppointmentViewModel;
        Close = new DelegateCommand(o => CloseWindow(true));
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
