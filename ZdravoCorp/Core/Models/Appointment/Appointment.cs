using System;
using System.Text.Json.Serialization;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.TimeSlots;

namespace ZdravoCorp.Core.Models.Appointment;


public class Appointment                    
{
    [JsonPropertyName("Id")] public int Id { get; set; }
    [JsonPropertyName("Time")] public TimeSlot Time { get; set; }
    [JsonPropertyName("Doctor")] public Doctor Doctor { get; set; }
    [JsonPropertyName("PatientEmail")] public string PatientEmail { get; set; }
    public String? Anamnesis { get; set; }
    public Room.Room? Room { get; set; }
    public bool IsCanceled;

    [JsonConstructor]
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


}