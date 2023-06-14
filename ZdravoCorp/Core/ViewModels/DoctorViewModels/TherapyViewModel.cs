using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Therapies;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class TherapyViewModel
{
    private Therapy _therapy;
    public string Description => _therapy.Description;
    public string Status =>  _therapy.Status? "Vazi" : "Ne vazi";
    public TherapyViewModel(Therapy therapy)
    {
        _therapy = therapy;
    }
}
