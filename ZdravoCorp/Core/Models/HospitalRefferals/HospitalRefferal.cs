using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Utilities;
using Newtonsoft.Json;
using ZdravoCorp.Core.Models.Therapies;

namespace ZdravoCorp.Core.Models.HospitalRefferals;

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
