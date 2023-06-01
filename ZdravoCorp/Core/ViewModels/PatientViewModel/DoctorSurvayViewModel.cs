﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Autofac;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Survays;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Services.DoctorServices;
using ZdravoCorp.Core.Services.ServayServices;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.ViewModels.PatientViewModel;

public class DoctorSurvayViewModel : ViewModelBase
{
    private ISurvayService _survayService;
    private IDoctorService _doctorService;
    private Doctor? _doctor;
    private string _patientEmail;
    private string _selectedGrade;
    private bool _yesChecked;
    private string _comment = "";

    public ICommand SubmitSurvayCommand { get; set; }
    public DoctorSurvayViewModel(IDoctorService doctorService,string doctorEmail, string patientEmail)
    {
        _survayService = Injector.Container.Resolve<ISurvayService>();
        _doctorService = doctorService;
        _doctor = doctorService.GetByEmail(doctorEmail);
        _patientEmail = patientEmail;
        PossibleGrades = new HashSet<string> { "1", "2", "3", "4", "5" };
        FillInputFields();

        SubmitSurvayCommand = new DelegateCommand(o => SubmitSurvayComm());
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
        var survay = _survayService.FindExistingDoctorSurvay(_doctor.Email,_patientEmail);
        if (survay == null)
            FillInputFieldsDefault();
        else
            FillInputFieldsWithSurvay(survay);
    }

    private void FillInputFieldsDefault()
    {
        _selectedGrade = "5";
        _yesChecked = true;
    }

    private void FillInputFieldsWithSurvay(DoctorSurvay survay)
    {
        _selectedGrade = survay.Grade.ToString();
        _yesChecked = survay.Recommendation;
        _comment = survay.Comment;
    }

    private void SubmitSurvayComm()
    {
        var survay = new DoctorSurvayDTO(IDGenerator.GetId(), _doctor.Email, _patientEmail,
            int.Parse(SelectedGrade), YesChecked, Comment);
        _survayService.AddDoctorSuvay(survay);
        MessageBox.Show("Survay created successfully", "Success", MessageBoxButton.OK);
        var doctorsAvgGrade = _survayService.FindAverageGradeForDoctor(_doctor.Email);
        _doctorService.UpdateGrade(_doctor.Email,doctorsAvgGrade);
    }

}