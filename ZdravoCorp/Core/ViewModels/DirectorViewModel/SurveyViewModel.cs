using ZdravoCorp.Core.Models.Survays;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class SurveyViewModel : ViewModelBase
{
    private readonly HospitalSurvay _survey;


    public SurveyViewModel(HospitalSurvay survey)
    {
        _survey = survey;
    }

    public string? Id => "id";
    public string User => _survey.PatientEmail;
    public string Recommendation => _survey.Recommendation.ToString();
}