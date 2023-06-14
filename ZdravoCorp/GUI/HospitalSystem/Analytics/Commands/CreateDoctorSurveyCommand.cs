using System;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalSystem.Analytics.Models;
using ZdravoCorp.Core.HospitalSystem.Analytics.Services;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.GUI.HospitalSystem.Analytics.ViewModels;

namespace ZdravoCorp.GUI.HospitalSystem.Analytics.Commands;

public class CreateDoctorSurveyCommand : ICommand
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

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        var survey = new DoctorSurveyDTO(IDGenerator.GetId(), _doctorEmail, _patientEmail,
            int.Parse(_createDoctorSurveyViewModel.SelectedGrade),_createDoctorSurveyViewModel.YesChecked, _createDoctorSurveyViewModel.Comment);
        _surveyService.AddDoctorSurvey(survey);
        MessageBox.Show("Survay created successfully", "Success", MessageBoxButton.OK);
        var doctorsAvgGrade = _surveyService.FindAverageGradeForDoctor(_doctorEmail);
        _doctorService.UpdateGrade(_doctorEmail, doctorsAvgGrade);
    }

    public event EventHandler? CanExecuteChanged;
}