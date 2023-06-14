using ZdravoCorp.Core.HospitalSystem.Analytics.Models;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalSystem.Analytics.ViewModels;

public class HospitalSurveyViewModel : ViewModelBase
{
    private readonly HospitalSurvey _survey;


    public HospitalSurveyViewModel(HospitalSurvey survey)
    {
        _survey = survey;
    }
    
    public string User => _survey.PatientEmail;
    public string Recommendation => _survey.Recommendation.ToString();
}