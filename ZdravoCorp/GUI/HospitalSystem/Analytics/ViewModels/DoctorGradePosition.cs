using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalSystem.Analytics.ViewModels;

public class DoctorGradePosition : ViewModelBase
{
    public string Position { get; set; }
    public string Doctor { get; set; }

    public DoctorGradePosition(string position, string doctor)
    {
        Position = position;
        Doctor = doctor;
    }
}