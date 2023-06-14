using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.HospitalRefferals;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class HospitalRefferalViewModel :ViewModelBase
{
    private HospitalRefferal _hospitalRefferal;
    public string PatientMail => _hospitalRefferal.PatientMail;
    public string StartTime => _hospitalRefferal.Time.Start.ToString();
    public string EndTime => _hospitalRefferal.Time.End.ToString();

    public HospitalRefferalViewModel(HospitalRefferal hospitalRefferal)
    {
        _hospitalRefferal = hospitalRefferal;
    }
}
