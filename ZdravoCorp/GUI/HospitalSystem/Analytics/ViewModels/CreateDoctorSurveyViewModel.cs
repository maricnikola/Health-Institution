using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Autofac;
using ZdravoCorp.Core.HospitalSystem.Analytics.Models;
using ZdravoCorp.Core.HospitalSystem.Analytics.Services;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.GUI.HospitalSystem.Analytics.Commands;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalSystem.Analytics.ViewModels;

public class CreateDoctorSurveyViewModel : ViewModelBase
{
    private ISurveyService _surveyService;
    private IDoctorService _doctorService;
    private Doctor? _doctor;
    private string _patientEmail;
    private string _selectedGrade;
    private bool _yesChecked;
    private bool _noChecked;
    private string _comment = "";

    public ICommand SubmitSurveyCommand { get; set; }
    public CreateDoctorSurveyViewModel(IDoctorService doctorService,string doctorEmail, string patientEmail)
    {
        _surveyService = Injector.Container.Resolve<ISurveyService>();
        _doctorService = doctorService;
        _doctor = doctorService.GetByEmail(doctorEmail);
        _patientEmail = patientEmail;
        PossibleGrades = new HashSet<string> { "1", "2", "3", "4", "5" };
        FillInputFields();

        SubmitSurveyCommand =
            new CreateDoctorSurveyCommand(this, _surveyService, _doctorService, doctorEmail, _patientEmail);
        //SubmitSurveyCommand = new DelegateCommand(o => SubmitSurveyComm());
    }

    public string DoctorName => _doctor.FullName;
    public string DoctorSpecialization => _doctor.Specialization.ToString();
    public HashSet<string> PossibleGrades { get; set; }
    public string SelectedGrade
    {
        get => _selectedGrade;
        set
        {
            _selectedGrade = value;
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
    public string Comment
    {
        get => _comment;
        set
        {
            _comment = value;
            OnPropertyChanged();
        }
    }

    private void FillInputFields()
    {
        var survey = _surveyService.FindExistingDoctorSurvey(_doctor.Email,_patientEmail);
        if (survey == null)
            FillInputFieldsDefault();
        else
            FillInputFieldsWithSurvey(survey);
    }

    private void FillInputFieldsDefault()
    {
        _selectedGrade = "5";
        _yesChecked = true;
    }

    private void FillInputFieldsWithSurvey(DoctorSurvey survey)
    {
        _selectedGrade = survey.Grade.ToString();
        _yesChecked = survey.Recommendation;
        _noChecked = !_yesChecked;
        _comment = survey.Comment;
    }

    private void SubmitSurveyComm()
    {
        var survey = new DoctorSurveyDTO(IDGenerator.GetId(), _doctor.Email, _patientEmail,
            int.Parse(SelectedGrade), YesChecked, Comment);
        _surveyService.AddDoctorSurvey(survey);
        MessageBox.Show("Survey created successfully", "Success", MessageBoxButton.OK);
        var doctorsAvgGrade = _surveyService.FindAverageGradeForDoctor(_doctor.Email);
        _doctorService.UpdateGrade(_doctor.Email,doctorsAvgGrade);
    }

}