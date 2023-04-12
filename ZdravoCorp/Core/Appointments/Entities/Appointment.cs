using System;
using ZdravoCorp.Core.TimeSlots;
using ZdravoCorp.Core.User;
using ZdravoCorp.Core.MedicalRecords.Model;
using ZdravoCorp.Core.Rooms;
using System.Security.Policy;
using System.Text.Json.Serialization;

namespace ZdravoCorp.Core.Appointments.Entities;


public class Appointment                    
{
    [JsonPropertyName("Id")] public int Id { get; set; }
    [JsonPropertyName("Time")] public TimeSlot Time { get; set; }
    [JsonPropertyName("Doctor")] public Doctor Doctor { get; set; }
    [JsonPropertyName("MedicalRecord")] public MedicalRecord MedicalRecord { get; set; }
    public String? Anamnesis { get; set; }
    public Room? Room { get; set; }
    public bool IsCanceled;

    [JsonConstructor]
    public Appointment(int id, TimeSlot t, Doctor doctor, MedicalRecord mr)
    {
        Id = id;
        Time = t;
        Doctor = doctor;
        MedicalRecord = mr;
        Anamnesis = null;
        Room = null;
        IsCanceled = false;
    }


}