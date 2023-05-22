using System;
using ZdravoCorp.Core.Models.AnamnesisReport;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.View;

namespace ZdravoCorp.Core.Models.Appointments;

public class AppointmentDTO
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Doctor { get; set; }
    public string PatientEmail { get; set; }
    public string Anamnesis { get; set; }
    public bool Status { get; set; }

    public AppointmentDTO(int id, DateTime date, string doctor, string patientEmail, string anamnesis)
    {
        Id = id;
        Date = date;
        Doctor = doctor;
        PatientEmail = patientEmail;
        Anamnesis = anamnesis;
    }
}