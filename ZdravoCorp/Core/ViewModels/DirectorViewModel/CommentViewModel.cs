using ZdravoCorp.Core.Models.Surveys;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class CommentViewModel : ViewModelBase
{
    private HospitalSurvey _survey;
    public CommentViewModel(HospitalSurvey survey)
    {
        _survey = survey;
    }

    public string User => _survey.PatientEmail;
    public string Text => _survey.Comment;
}