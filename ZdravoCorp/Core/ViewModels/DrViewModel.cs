using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.ViewModels;

public class DrViewModel
{
    private readonly Doctor _doctor;

    public string DoctorName => _doctor.FullName;

    public DrViewModel(Doctor appointment)
    {
        _doctor = appointment;
    }
}