using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Presriptions;

namespace ZdravoCorp.Core.ViewModels;

public class PrescriptionViewModel
{
    private readonly Prescription _prescription;

    public PrescriptionViewModel(Prescription prescription)
    {
        _prescription = prescription;
    }

    public string Medicament => _prescription.Medicament;
    public int TimesADay => _prescription.TimesADay;
    public string Instructions => _prescription.Instructions;


}
