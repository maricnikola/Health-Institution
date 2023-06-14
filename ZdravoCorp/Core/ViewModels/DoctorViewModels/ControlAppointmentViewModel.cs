using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.HospitalRefferals;
using ZdravoCorp.Core.Services.HospitalRefferalServices;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class ControlAppointmentViewModel:ViewModelBase
{
    public ICommand Yes { get;set;}
    public ICommand No { get; set; }
    public IHospitalRefferalService _hospitalRefferalService;
    public HospitalRefferal _hospitalRefferal;
    public HospitalizedPatientsViewModel _hospitalizedPatientsViewModel;
    public ControlAppointmentViewModel(IHospitalRefferalService hospitalRefferalService,HospitalRefferal hospitalRefferal,
        HospitalizedPatientsViewModel hospitalizedPatientsViewModel)
    {
        _hospitalizedPatientsViewModel = hospitalizedPatientsViewModel;
        _hospitalRefferalService = hospitalRefferalService;
        _hospitalRefferal = hospitalRefferal;
        Yes = new DelegateCommand(o => AddControlAppointment());
        No = new DelegateCommand(o => DeleteControlAppointment());
    }

    private void CloseWindow()
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }
    public void AddControlAppointment()
    {
        _hospitalRefferalService.UpdateControlAppointment(_hospitalRefferal.Id,true);
        _hospitalizedPatientsViewModel.ShowHospitalizedPatients();
        CloseWindow();
    }
    public void DeleteControlAppointment()
    {
        _hospitalRefferalService.UpdateControlAppointment(_hospitalRefferal.Id, false);
        _hospitalizedPatientsViewModel.ShowHospitalizedPatients();
        CloseWindow();
    }
}
