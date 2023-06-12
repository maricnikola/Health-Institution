using ZdravoCorp.Core.Models.Surveys;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class SurveyViewModel : ViewModelBase
{
    private readonly HospitalSurvey _survey;


    public SurveyViewModel(HospitalSurvey survey)
    {
        _survey = survey;
    }

    public string? Id => "id";
    public string User => _survey.PatientEmail;
    public string Recommendation => _survey.Recommendation.ToString();
}