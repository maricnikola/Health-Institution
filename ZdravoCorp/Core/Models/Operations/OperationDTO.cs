using System.Collections.Generic;
using ZdravoCorp.Core.Models.Equipments;
using ZdravoCorp.Core.Models.MedicalRecords;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Models.Operations;

public class OperationDTO
{
    public int Id { get; set; }
    public TimeSlot Time { get; set; }
    public Doctor Doctor { get; set; }
    public MedicalRecord MedicalRecord { get; set; }
    public Room? Room { get; set; }
    public List<Equipment>? Equipment { get; set; }

    public OperationDTO(int id, TimeSlot time, Doctor doctor, MedicalRecord medicalRecord, Room? room, List<Equipment>? equipment)
    {
        Id = id;
        Time = time;
        Doctor = doctor;
        MedicalRecord = medicalRecord;
        Room = room;
        Equipment = equipment;
    }
}