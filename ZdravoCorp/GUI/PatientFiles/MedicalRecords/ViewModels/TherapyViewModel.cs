using ZdravoCorp.Core.PatientFiles.Refferals.Models;

namespace ZdravoCorp.GUI.PatientFiles.MedicalRecords.ViewModels;

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
