using Newtonsoft.Json;
using ZdravoCorp.Core.Models.AnamnesisReport;
using ZdravoCorp.Core.Models.Presriptions;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Models.Appointments;

public class Appointment
{
    public Appointment(int id, TimeSlot t, Doctor doctor, string email)
    {
        Id = id;
        Time = t;
        Doctor = doctor;
        PatientEmail = email;
        Anamnesis = null;
        Room = null;
        IsCanceled = false;
    }

    [JsonConstructor]
    public Appointment(int id, TimeSlot t, Doctor doctor, string email, Anamnesis anamnesis)
    {
        Id = id;
        Time = t;
        Doctor = doctor;
        PatientEmail = email;
        Anamnesis = anamnesis;
        Room = null;
        IsCanceled = false;
        Status = false;
        Prescription = null;
    }

    public int Id { get; set; }
    public TimeSlot Time { get; set; }
    public Doctor Doctor { get; set; }
    public string PatientEmail { get; set; }
    public Anamnesis Anamnesis { get; set; }
    public int? Room { get; set; }
    public bool IsCanceled { get; set; }
    public bool Status { get; set; }
    public Prescription Prescription { get; set; }

    public Appointment(int id, TimeSlot t, Doctor doctor, string email, Anamnesis anamnesis, Prescription prescription)
    {
        Id = id;
        Time = t;
        Doctor = doctor;
        PatientEmail = email;
        Anamnesis = anamnesis;
        Room = null;
        IsCanceled = false;
        Status = false;
        Prescription = prescription;
    }
    public Appointment(AppointmentDTO appointmentDto)
    {
        Doctor = appointmentDto.Doctor;
        Anamnesis = null;
        IsCanceled = false;
        PatientEmail = appointmentDto.PatientEmail;
        Id = appointmentDto.Id;
        Room = null;
        Status = appointmentDto.Status;
        var endTime = appointmentDto.Date.AddMinutes(15);
        Time = new TimeSlot(appointmentDto.Date, endTime);
        Prescription = null;
    }
}