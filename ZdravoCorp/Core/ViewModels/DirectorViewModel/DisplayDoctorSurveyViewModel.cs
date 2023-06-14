﻿using System;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Surveys;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DisplayDoctorSurveyViewModel : ViewModelBase
{
    private DoctorSurvey _survey;
    public string Patient => _survey.PatientEmail;
    public string Doctor => _survey.DoctorEmail;

    public int Grade => _survey.Grade;
    public bool Recommendation => _survey.Recommendation;
    public string Comment => _survey.Comment;
    
    public event EventHandler? OnRequestClose;

    private void Exit()
    {
        OnRequestClose?.Invoke(this, new EventArgs());
    }

    public ICommand ExitSurvey { get; set; }
    public DisplayDoctorSurveyViewModel(DoctorSurvey survey)
    {
        _survey = survey;
        ExitSurvey = new DelegateCommand(o => Exit());
    }
}