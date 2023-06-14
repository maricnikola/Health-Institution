using System;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.HospitalSystem.Analytics.Models;
using ZdravoCorp.Core.HospitalSystem.Analytics.Services;
using ZdravoCorp.GUI.HospitalSystem.Analytics.ViewModels;

namespace ZdravoCorp.GUI.HospitalSystem.Analytics.Commands;

public class CreateHospitalSurveyCommad : ICommand
{
    private CreateHospitalSurveyViewModel _createHospitalSurveyViewModel;
    private ISurveyService _surveyService;
    private string _patientEmail;

    public CreateHospitalSurveyCommad(CreateHospitalSurveyViewModel createHospitalSurveyViewModel, ISurveyService service ,string patientEmail)
    {
        _createHospitalSurveyViewModel = createHospitalSurveyViewModel;
        _patientEmail = patientEmail;
        _surveyService = service;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        var survey =
            new HospitalSurveyDTO(_patientEmail, int.Parse(_createHospitalSurveyViewModel.ServiceGrade),
                int.Parse(_createHospitalSurveyViewModel.HygieneGrade),
                int.Parse(_createHospitalSurveyViewModel.OverallGrade), _createHospitalSurveyViewModel.YesChecked,
                _createHospitalSurveyViewModel.Comment);
        _surveyService.AddHospitalSurvey(survey);
        MessageBox.Show("Survay added successfully", "Success", MessageBoxButton.OK);
    }

    public event EventHandler? CanExecuteChanged;
}