﻿using System;
using ZdravoCorp.Core.Scheduling.Models;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.Scheduling.ViewModels;

public class AppointmentViewModel : ViewModelBase
{
    private readonly Appointment _appointment;


    public AppointmentViewModel(Appointment appointment)
    {
        // PatientName = patientFullName;
        _appointment = appointment;
        if (appointment.Anamnesis != null)
        {
            Anamnesis = appointment.Anamnesis.KeyWord;
        }
        else
            Anamnesis = "";
    }

    public int Id => _appointment.Id;
    public string DoctorName => _appointment.Doctor.FullName;
    public string DoctorEmail => _appointment.Doctor.Email;
    public DateTime Date => _appointment.Time.Start;
    public string Anamnesis { get; set; }
    public string PatientMail => _appointment.PatientEmail;
    public string Specialization => _appointment.Doctor.Specialization.ToString();

    public override string ToString()
    {
        return $"{Date,-25} | {DoctorName,-25} | {PatientMail,-25}";
        //return String.Format("{0,-10} | {1, -15} | {2, 10}", Date, DoctorName, PatientMail, Id);
    }
}