using Newtonsoft.Json;
using ZdravoCorp.Core.Models.AnamnesisReport;
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
    }

    public int Id { get; set; }
    public TimeSlot Time { get; set; }
    public Doctor Doctor { get; set; }
    public string PatientEmail { get; set; }
    public Anamnesis Anamnesis { get; set; }
    public int? Room { get; set; }
    public bool IsCanceled { get; set; }
    public bool Status { get; set; }


    public Appointment(AppointmentDTO appointmentDto)
    {
        Doctor = appointmentDto.Doctor;
        Anamnesis = appointmentDto.Anamnesis;
        IsCanceled = appointmentDto.IsCanceled;
        PatientEmail = appointmentDto.PatientEmail;
        Id = appointmentDto.Id;
        Room = appointmentDto.Room;
        Status = appointmentDto.Status;
        Time = appointmentDto.Time;
    }
}