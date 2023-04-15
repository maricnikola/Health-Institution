using System;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.View;

namespace ZdravoCorp.Core.ViewModels;

public class AppointmentViewModel : ViewModelBase
{
    private readonly Appointment _appointment;

    public int Id => _appointment.Id;
    public string DoctorName => _appointment.Doctor.FullName;
    public string PatientName => _appointment.MedicalRecord.user.FullName;
    public DateTime Date => _appointment.Time.start;

    public AppointmentViewModel(Appointment appointment)
    {
        _appointment = appointment;
    }



}