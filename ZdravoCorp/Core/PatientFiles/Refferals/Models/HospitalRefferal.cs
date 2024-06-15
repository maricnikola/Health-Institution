using System.Collections.Generic;
using Newtonsoft.Json;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.PatientFiles.Refferals.Models;

public class HospitalRefferal
{
    public int Id { get; set; }
    public string PatientMail { get; set; }
    public TimeSlot Time { get; set; }
    public List<Therapy> InitialTherapy { get; set; }
    public string? AdditionalTests { get; set; }
    public int RoomId { get; set; }
    public bool ControlAppointment { get; set; }

    [JsonConstructor]
    public HospitalRefferal(int id, string patientMail, TimeSlot time, List<Therapy> initialTherapy,string additionalTests, int roomId)
    {
        Id = id;
        PatientMail = patientMail;
        Time = time;
        InitialTherapy = initialTherapy;
        AdditionalTests = additionalTests;
        RoomId = roomId;
        ControlAppointment = false;
    }
}
