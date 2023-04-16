using System.Collections.Generic;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Models.Room;
using ZdravoCorp.Core.TimeSlots;

namespace ZdravoCorp.Core.Models.Operation;

public class Operation
{
    public int Id { get; set; }
    public TimeSlot Time { get; set; }
    public Doctor doctor { get; set; }
    public MedicalRecord.MedicalRecord medicalRecord { get; set; }
    public Room.Room? room { get; set; }
    public List<Equipment.Equipment>? equipment { get; set; }
    public bool IsCanceled;
    public Operation(int id, TimeSlot time, Doctor doctor, MedicalRecord.MedicalRecord medicalRecord)
    {
        Id = id;
        Time = time;
        doctor = doctor;
        medicalRecord = medicalRecord;
        room = null;
        equipment = null;
        IsCanceled = false; 
    }
    
    
}