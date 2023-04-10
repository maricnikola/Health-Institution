using System;
using System.Windows.Documents;
using ZdravoCorp.Core.MedicalRecords.Model;
using ZdravoCorp.Core.Rooms;
using ZdravoCorp.Core.TimeSlots;
using ZdravoCorp.Core.User;
using ZdravoCorp.Core.Equipments;
using System.Collections.Generic;
using ZdravoCorp.Core.Equipments.Model;

namespace ZdravoCorp.Core.Operations.Model;

public class Operation
{
    public int Id { get; set; }
    public TimeSlot Time { get; set; }
    public Doctor doctor { get; set; }
    public MedicalRecord medicalRecord { get; set; }
    public Room? room { get; set; }
    public List<Equipment>? equipment { get; set; }
    public bool IsCanceled;
    public Operation(int id, TimeSlot time, Doctor doctor, MedicalRecord medicalRecord)
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