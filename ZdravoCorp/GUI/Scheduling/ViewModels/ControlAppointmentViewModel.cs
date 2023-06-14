using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.PatientFiles.Refferals.Models;
using ZdravoCorp.Core.PatientFiles.Refferals.Services;
using ZdravoCorp.GUI.Main;
using ZdravoCorp.GUI.PatientFiles.MedicalRecords.ViewModels;

namespace ZdravoCorp.GUI.Scheduling.ViewModels;

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
