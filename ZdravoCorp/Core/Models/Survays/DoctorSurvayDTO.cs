using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Models.Survays;

public class DoctorSurvayDTO
{
    public int Id { get; set; }
    public string DoctorEmail { get; set; }
    public string PatientEmail { get; set; }
    public int Grade { get; set; }
    public bool Recommendation { get; set; }
    public string Comment { get; set; }

    public DoctorSurvayDTO(int id,string doctorEmail, string patientEmail, int grade, bool recommendation, string comment)
    {
        Id = id;
        DoctorEmail = doctorEmail;
        PatientEmail = patientEmail;
        Grade = grade;
        Recommendation = recommendation;
        Comment = comment;
    }
}