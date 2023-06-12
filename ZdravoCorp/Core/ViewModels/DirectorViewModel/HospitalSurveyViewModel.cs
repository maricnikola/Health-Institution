using ZdravoCorp.Core.Models.Surveys;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

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