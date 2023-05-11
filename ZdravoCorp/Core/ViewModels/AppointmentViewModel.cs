using System;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.View;

namespace ZdravoCorp.Core.ViewModels;

public class AppointmentViewModel : ViewModelBase
{
    private readonly Appointment _appointment;

    public int Id => _appointment.Id;
    public string DoctorName => _appointment.Doctor.FullName;
    
    public string PatientName => _appointment.PatientEmail;
    public DateTime Date => _appointment.Time.start;
    public string PatientMail => _appointment.PatientEmail;


    public AppointmentViewModel(Appointment appointment)
    {
       // PatientName = patientFullName;
        _appointment = appointment;
    }
}