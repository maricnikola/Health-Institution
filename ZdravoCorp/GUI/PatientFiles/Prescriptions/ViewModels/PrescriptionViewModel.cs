using ZdravoCorp.Core.PatientFiles.Presriptions.Models;

namespace ZdravoCorp.GUI.PatientFiles.Prescriptions.ViewModels;

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
