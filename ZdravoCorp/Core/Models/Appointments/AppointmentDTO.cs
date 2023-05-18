using ZdravoCorp.Core.Models.AnamnesisReport;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.View;

namespace ZdravoCorp.Core.Models.Appointments;

public class AppointmentDTO
{
    public int Id { get; set; }
    public TimeSlot Time { get; set; }
    public Doctor Doctor { get; set; }
    public string PatientEmail { get; set; }
    public Anamnesis Anamnesis { get; set; }
    public int? Room { get; set; }
    public bool IsCanceled { get; set; }
    public bool Status { get; set; }

    public AppointmentDTO(int id, TimeSlot time, Doctor doctor, string patientEmail, Anamnesis anamnesis, int? room, bool isCanceled, bool status)
    {
        Id = id;
        Time = time;
        Doctor = doctor;
        PatientEmail = patientEmail;
        Anamnesis = anamnesis;
        Room = room;
        IsCanceled = isCanceled;
        Status = status;
    }
}