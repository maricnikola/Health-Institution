using ZdravoCorp.Core.Models.Survays;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class CommentViewModel : ViewModelBase
{
    private HospitalSurvay _survey;
    public CommentViewModel(HospitalSurvay survey)
    {
        _survey = survey;
    }

    public string User => _survey.PatientEmail;
    public string Text => _survey.Comment;
}