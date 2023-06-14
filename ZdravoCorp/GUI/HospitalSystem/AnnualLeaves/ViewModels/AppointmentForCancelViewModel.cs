using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalSystem.AnnualLeaves.ViewModels;

public class AppointmentForCancelViewModel : ViewModelBase
{
    public int Id { get; }
    public string Patient { get; }
    public string Date { get; }

    public AppointmentForCancelViewModel(int id, string patient, string date)
    {
        Id = id;
        Patient = patient;
        Date = date;
    }
}