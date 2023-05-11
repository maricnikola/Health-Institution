using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.ViewModels;

public class PatientsViewModel :ViewModelBase
{

    private readonly Patient _patient;

    public string FirstName => _patient.FirstName;
    public string LastName => _patient.LastName;
    public string Email => _patient.Email;


    public PatientsViewModel(Patient patient)
    {
        _patient = patient;
    }
    public override string ToString()
    {
        return $"{nameof(FirstName)}: {FirstName}, {nameof(LastName)}: {LastName}, {nameof(Email)}: {Email}";
    }

}
