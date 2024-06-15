using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalSystem.Analytics.Models;
using ZdravoCorp.Core.HospitalSystem.Analytics.Services;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.GUI.HospitalSystem.Analytics.ViewModels;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.HospitalSystem.Analytics.Commands;

public class CreateDoctorSurveyCommand : CommandBase
{
    private CreateDoctorSurveyViewModel _createDoctorSurveyViewModel;
    private ISurveyService _surveyService;
    private IDoctorService _doctorService;
    private string _doctorEmail;
    private string _patientEmail;

    public CreateDoctorSurveyCommand(CreateDoctorSurveyViewModel createDoctorSurveyViewModel, ISurveyService surveyService, IDoctorService doctorService, string doctorEmail, string patientEmail)
    {
        _createDoctorSurveyViewModel = createDoctorSurveyViewModel;
        _surveyService = surveyService;
        _doctorService = doctorService;
        _doctorEmail = doctorEmail;
        _patientEmail = patientEmail;
    }

    public override bool CanExecute(object? parameter)
    {
        return true;
    }
    public override void Execute(object? parameter)
    {
        var survey = new DoctorSurveyDTO(IDGenerator.GetId(), _doctorEmail, _patientEmail,
            int.Parse(_createDoctorSurveyViewModel.SelectedGrade),_createDoctorSurveyViewModel.YesChecked, _createDoctorSurveyViewModel.Comment);
        _surveyService.AddDoctorSurvey(survey);
        var doctorsAvgGrade = _surveyService.FindAverageGradeForDoctor(_doctorEmail);
        _doctorService.UpdateGrade(_doctorEmail, doctorsAvgGrade);
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }

    public event EventHandler? CanExecuteChanged;
}