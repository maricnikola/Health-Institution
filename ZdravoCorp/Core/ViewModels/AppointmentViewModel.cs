using System;
using ZdravoCorp.Core.Models.Appointments;

namespace ZdravoCorp.Core.ViewModels;

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
    public string DoctorName => _appointment.Doctor.Email;
    public string PatientName => _appointment.PatientEmail;
    public DateTime Date => _appointment.Time.Start;
    public string Anamnesis { get; set; }
    public string PatientMail => _appointment.PatientEmail;
    public string Specialization => _appointment.Doctor.Specialization.ToString();
}