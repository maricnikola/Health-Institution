using System;
using ZdravoCorp.Core.TimeSlots;
using ZdravoCorp.Core.User;
using ZdravoCorp.Core.MedicalRecords.Model;
using ZdravoCorp.Core.Rooms;
using System.Security.Policy;

namespace ZdravoCorp.Core.Appointments.Model;


public class Appointment                    
{
    public int Id { get; set; }
    public TimeSlot Time { get; set; }
    public Doctor Doctor { get; set; }
    public MedicalRecord MedicalRecord { get; set; }
    public String? Anamnesis { get; set; }
    public Room? Room { get; set; }
    public bool IsCanceled;

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