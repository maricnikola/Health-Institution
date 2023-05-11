using System;
using ZdravoCorp.Core.Models.AnamnesisReport;
using ZdravoCorp.Core.TimeSlots;
using Newtonsoft.Json;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Models.Rooms;

namespace ZdravoCorp.Core.Models.Appointment;

public class Appointment
{
    public int Id { get; set; }
    public TimeSlot Time { get; set; }
    public Doctor Doctor { get; set; }
    public string PatientEmail { get; set; }
    public Anamnesis Anamnesis { get; set; }
    public int? Room { get; set; }
    public bool IsCanceled { get; set; }
    public bool Status { get; set; }

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
    public Appointment(int id, TimeSlot t, Doctor doctor, string email, AnamnesisReport.Anamnesis anamnesis)
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
}