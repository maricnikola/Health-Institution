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
    public Doctor Doctor { get; set; }
    public string PatientEmail { get; set; }
    public Anamnesis Anamnesis { get; set; }
    public bool Status { get; set; }



    public AppointmentDTO(int id, DateTime date, Doctor doctor, string patientEmail, Anamnesis anamnesis)
    {
        Id = id;
        Date = date;
        Doctor = doctor;
        PatientEmail = patientEmail;
        Anamnesis = anamnesis;
    }
}