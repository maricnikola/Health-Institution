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
    [JsonPropertyName("MedicalRecord")] public MedicalRecord.MedicalRecord MedicalRecord { get; set; }
    public String? Anamnesis { get; set; }
    public Room.Room? Room { get; set; }
    public bool IsCanceled;

    [JsonConstructor]
    public Appointment(int id, TimeSlot t, Doctor doctor, MedicalRecord.MedicalRecord mr)
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