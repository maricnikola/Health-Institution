using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Models.Survays;
using ZdravoCorp.Core.Services.ServayServices;
using ZdravoCorp.Core.ViewModels.PatientViewModel;

namespace ZdravoCorp.Core.Commands;

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
        _surveyService.AddHospitalSurvay(survey);
        MessageBox.Show("Survay added successfully", "Success", MessageBoxButton.OK);
    }

    public event EventHandler? CanExecuteChanged;
}