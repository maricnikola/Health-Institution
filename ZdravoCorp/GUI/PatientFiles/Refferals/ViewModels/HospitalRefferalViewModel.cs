using ZdravoCorp.Core.PatientFiles.Refferals.Models;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.PatientFiles.Refferals.ViewModels;

public class HospitalRefferalViewModel :ViewModelBase
{
    private HospitalRefferal _hospitalRefferal;
    public string PatientMail => _hospitalRefferal.PatientMail;
    public string StartTime => _hospitalRefferal.Time.Start.ToString();
    public string EndTime => _hospitalRefferal.Time.End.ToString();
    public int Id => _hospitalRefferal.Id;
    public string ControlAppointment => _hospitalRefferal.ControlAppointment ? "Zakazana" : "Nije zakazana"; 
    public HospitalRefferalViewModel(HospitalRefferal hospitalRefferal)
    {
        _hospitalRefferal = hospitalRefferal;
    }
}
