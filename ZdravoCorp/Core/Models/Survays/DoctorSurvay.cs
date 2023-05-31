using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Models.Survays;

public class DoctorSurvay
{
    public int Id { get; set; }
    public string DoctorEmail { get; set; }
    public string PatientEmail { get; set; }
    public int Grade { get; set; }
    public bool Recommendation { get; set; }
    public string Comment { get; set; }

    [JsonConstructor]
    public DoctorSurvay(int id,string doctorEmail, string patientEmail, int grade, bool recommendation, string comment)
    {
        Id = id;
        DoctorEmail = doctorEmail;
        PatientEmail = patientEmail;
        Grade = grade;
        Recommendation = recommendation;
        Comment = comment;
    }

    public DoctorSurvay(DoctorSurvayDTO doctorSurvayDto)
    {
        Id = doctorSurvayDto.Id;
        DoctorEmail = doctorSurvayDto.DoctorEmail;
        PatientEmail = doctorSurvayDto.PatientEmail;
        Grade = doctorSurvayDto.Grade;
        Recommendation = doctorSurvayDto.Recommendation;
        Comment = doctorSurvayDto.Comment;
    }
}