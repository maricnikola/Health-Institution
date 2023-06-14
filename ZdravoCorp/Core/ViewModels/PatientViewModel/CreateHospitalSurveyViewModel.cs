using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Surveys;
using ZdravoCorp.Core.Services.ServayServices;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class CreateHospitalSurveyViewModel : ViewModelBase
{
    private ISurveyService _surveyService;
    private string _patientEmail;
    private string _comment = "";
    private string _serviceGrade;
    private string _hygieneGrade;
    private string _overallGrade;
    private bool _yesChecked;
    private bool _noChecked;
    public ICommand CreateHospitalSurveyComm { get; set; }

    public CreateHospitalSurveyViewModel(ISurveyService surveyService, string patientEmail)
    {
        _surveyService = surveyService;
        _patientEmail = patientEmail;
        PossibleGrades = new HashSet<string> { "1", "2", "3", "4", "5" };
        FillInputFields();
        CreateHospitalSurveyComm = new CreateHospitalSurveyCommad(this, _surveyService, _patientEmail);
        //CreateHospitalSurvayCommand = new DelegateCommand(o => CreateHospitalSurvay());
    }

    public HashSet<string> PossibleGrades { get; set; }

    public string ServiceGrade
    {
        get => _serviceGrade;
        set
        {
            _serviceGrade = value;
            OnPropertyChanged();
        }
    }
    public string HygieneGrade
    {
        get => _hygieneGrade;
        set
        {
            _hygieneGrade = value;
            OnPropertyChanged();
        }
    }
    public string OverallGrade
    {
        get => _overallGrade;
        set
        {
            _overallGrade = value;
            OnPropertyChanged();
        }
    }
    public string Comment
    {
        get => _comment;
        set
        {
            _comment = value;
            OnPropertyChanged();
        }
    }
    public bool YesChecked
    {
        get => _yesChecked;
        set
        {
            _yesChecked = value;
            OnPropertyChanged();
        }
    }
    public bool NoChecked
    {
        get => _noChecked;
        set
        {
            _noChecked = value;
            OnPropertyChanged();
        }
    }

    private void FillInputFields()
    {
        var survey = _surveyService.FindHospitalSurveyForPatient(_patientEmail);
        if (survey == null)
            FillInputFieldsDefault();
        else
            FillInputFieldsWithSurvey(survey);
    }

    private void FillInputFieldsDefault()
    {
        _serviceGrade = "5";
        _hygieneGrade = "5";
        _overallGrade = "5";
        _yesChecked = true;
    }

    private void FillInputFieldsWithSurvey(HospitalSurvey survey)
    {
        _serviceGrade = survey.ServiceGrade.ToString();
        _hygieneGrade = survey.HygieneGrade.ToString();
        _overallGrade = survey.OverallGrade.ToString();
        _yesChecked = survey.Recommendation;
        _noChecked = !_yesChecked;
        _comment = survey.Comment;
    }

    public void CreateHospitalSurvey()
    {
        var survey =
            new HospitalSurveyDTO(_patientEmail, int.Parse(ServiceGrade), int.Parse(HygieneGrade),
                int.Parse(OverallGrade), YesChecked, Comment);
        _surveyService.AddHospitalSurvey(survey);
        MessageBox.Show("Survey added successfully", "Success", MessageBoxButton.OK);
    }

}