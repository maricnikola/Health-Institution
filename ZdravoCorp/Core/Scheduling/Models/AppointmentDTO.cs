using System;
using ZdravoCorp.Core.HospitalSystem.Users.Models;

namespace ZdravoCorp.Core.Scheduling.Models;

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