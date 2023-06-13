namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

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