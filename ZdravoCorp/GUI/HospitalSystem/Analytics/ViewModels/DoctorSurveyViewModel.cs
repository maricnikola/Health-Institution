using ZdravoCorp.Core.HospitalSystem.Analytics.Models;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalSystem.Analytics.ViewModels;

public class DoctorSurveyViewModel : ViewModelBase
{
    private DoctorSurvey _survey;
    public DoctorSurveyViewModel(DoctorSurvey survey)
    {
        _survey = survey;
    }
    public string Patient => _survey.PatientEmail;
    public string Doctor => _survey.DoctorEmail;


}