using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class PerformAppointmentViewModel
{

    public ICommand PerformCommand { get; }
    public ICommand CancelCommand { get; }

    public PerformAppointmentViewModel()
    {
        CancelCommand = new DelegateCommand(o => CloseWindow());
    }

    private void CloseWindow()
    {
        Window activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }
}
