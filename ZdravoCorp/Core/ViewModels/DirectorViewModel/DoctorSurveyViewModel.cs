using ZdravoCorp.Core.Models.Surveys;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

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