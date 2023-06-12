using System;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Surveys;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DisplayHospitalSurveyViewModel : ViewModelBase
{
    private HospitalSurvey _survey;
    public string User => _survey.PatientEmail;
    public int ServiceGrade => _survey.ServiceGrade;
    public int HygieneGrade => _survey.HygieneGrade;
    public int OverallGrade => _survey.OverallGrade;
    public bool Recommendation => _survey.Recommendation;
    public string Comment => _survey.Comment;
    
    public event EventHandler? OnRequestClose;

    private void Exit()
    {
        OnRequestClose?.Invoke(this, new EventArgs());
    }

    public ICommand ExitSurvey { get; set; }
    public DisplayHospitalSurveyViewModel(HospitalSurvey survey)
    {
        _survey = survey;
        ExitSurvey = new DelegateCommand(o => Exit());
    }
}