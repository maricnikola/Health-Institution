using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using ZdravoCorp.Core.Models.Survays;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Services.DoctorServices;
using ZdravoCorp.Core.Services.ServayServices;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.ViewModels.PatientViewModel;

namespace ZdravoCorp.Core.Commands;

public class CreateDoctorSurveyCommand : ICommand
{
    private DoctorSurveyViewModel _doctorSurveyViewModel;
    private ISurveyService _surveyService;
    private IDoctorService _doctorService;
    private string _doctorEmail;
    private string _patientEmail;

    public CreateDoctorSurveyCommand(DoctorSurveyViewModel doctorSurveyViewModel, ISurveyService surveyService, IDoctorService doctorService, string doctorEmail, string patientEmail)
    {
        _doctorSurveyViewModel = doctorSurveyViewModel;
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
            int.Parse(_doctorSurveyViewModel.SelectedGrade),_doctorSurveyViewModel.YesChecked, _doctorSurveyViewModel.Comment);
        _surveyService.AddDoctorSuvay(survey);
        MessageBox.Show("Survay created successfully", "Success", MessageBoxButton.OK);
        var doctorsAvgGrade = _surveyService.FindAverageGradeForDoctor(_doctorEmail);
        _doctorService.UpdateGrade(_doctorEmail, doctorsAvgGrade);
    }

    public event EventHandler? CanExecuteChanged;
}