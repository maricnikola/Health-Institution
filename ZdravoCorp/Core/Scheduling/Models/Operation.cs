using System.Collections.Generic;
using ZdravoCorp.Core.HospitalAssets.Equipment.Models;
using ZdravoCorp.Core.HospitalAssets.Rooms.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.PatientFiles.MedicalRecords.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Scheduling.Models;

public class Operation
{
    public readonly bool IsCanceled;

    public Operation(int id, TimeSlot time, Doctor doctor, MedicalRecord medicalRecord)
    {
        Id = id;
        Time = time;
        Doctor = doctor;
        MedicalRecord = medicalRecord;
        Doctor = doctor;
        MedicalRecord = medicalRecord;
        Room = null;
        Equipment = null;
        IsCanceled = false;
    }

    public int Id { get; set; }
    public TimeSlot Time { get; set; }
    public Doctor Doctor { get; set; }
    public MedicalRecord MedicalRecord { get; set; }
    public Room? Room { get; set; }
    public List<Equipment>? Equipment { get; set; }

    public Operation(OperationDTO operationDto)
    {
        Doctor = operationDto.Doctor;
        MedicalRecord = operationDto.MedicalRecord;
        Equipment = operationDto.Equipment;
        Id = operationDto.Id;
        Room = operationDto.Room;
        Time = operationDto.Time;
    }
}