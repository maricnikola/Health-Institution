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

public class AddSpecialistsRefferalViewModel : ViewModelBase
{
    private PerformAppointmentViewModel _performAppointmentViewModel;

    public AddSpecialistsRefferalViewModel(PerformAppointmentViewModel performAppointmentViewModel)
    {
        _performAppointmentViewModel = performAppointmentViewModel;

        CloseDialog = new DelegateCommand(o => CloseWindow());
    }



    public ICommand CloseDialog { get; }
    private void CloseWindow()
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
        var performWindow = new PerformAppointmentView() { DataContext = _performAppointmentViewModel };
        performWindow.Show();
    }
}
