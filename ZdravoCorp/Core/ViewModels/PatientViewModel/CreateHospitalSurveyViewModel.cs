using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Survays;
using ZdravoCorp.Core.Services.ServayServices;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class CreateHospitalSurveyViewModel : ViewModelBase
{
    private ISurveyService _survayService;
    private string _patientEmail;
    private string _comment = "";
    private string _serviceGrade;
    private string _hygieneGrade;
    private string _overallGrade;
    private bool _yesChecked;
    private bool _noChecked;
    public ICommand CreateHospitalSurveyComm { get; set; }

    public CreateHospitalSurveyViewModel(ISurveyService survayService, string patientEmail)
    {
        _survayService = survayService;
        _patientEmail = patientEmail;
        PossibleGrades = new HashSet<string> { "1", "2", "3", "4", "5" };
        FillInputFields();
        CreateHospitalSurveyComm = new CreateHospitalSurveyCommad(this, _survayService, _patientEmail);
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
        var survay = _survayService.FindHospitalSurvayForPatient(_patientEmail);
        if (survay == null)
            FillInputFieldsDefault();
        else
            FillInputFieldsWithSurvay(survay);
    }

    private void FillInputFieldsDefault()
    {
        _serviceGrade = "5";
        _hygieneGrade = "5";
        _overallGrade = "5";
        _yesChecked = true;
    }

    private void FillInputFieldsWithSurvay(HospitalSurvey survay)
    {
        _serviceGrade = survay.ServiceGrade.ToString();
        _hygieneGrade = survay.HygieneGrade.ToString();
        _overallGrade = survay.OverallGrade.ToString();
        _yesChecked = survay.Recommendation;
        _noChecked = !_yesChecked;
        _comment = survay.Comment;
    }

    public void CreateHospitalSurvay()
    {
        var survay =
            new HospitalSurveyDTO(_patientEmail, int.Parse(ServiceGrade), int.Parse(HygieneGrade),
                int.Parse(OverallGrade), YesChecked, Comment);
        _survayService.AddHospitalSurvay(survay);
        MessageBox.Show("Survay added successfully", "Success", MessageBoxButton.OK);
    }

}