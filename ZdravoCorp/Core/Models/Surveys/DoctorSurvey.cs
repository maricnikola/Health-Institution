using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Models.Surveys;

public class DoctorSurvey
{
    public int Id { get; set; }
    public string DoctorEmail { get; set; }
    public string PatientEmail { get; set; }
    public int Grade { get; set; }
    public bool Recommendation { get; set; }
    public string Comment { get; set; }

    [JsonConstructor]
    public DoctorSurvey(int id,string doctorEmail, string patientEmail, int grade, bool recommendation, string comment)
    {
        Id = id;
        DoctorEmail = doctorEmail;
        PatientEmail = patientEmail;
        Grade = grade;
        Recommendation = recommendation;
        Comment = comment;
    }

    public DoctorSurvey(DoctorSurveyDTO doctorSurveyDto)
    {
        Id = doctorSurveyDto.Id;
        DoctorEmail = doctorSurveyDto.DoctorEmail;
        PatientEmail = doctorSurveyDto.PatientEmail;
        Grade = doctorSurveyDto.Grade;
        Recommendation = doctorSurveyDto.Recommendation;
        Comment = doctorSurveyDto.Comment;
    }
}