using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Models.Survays;

public class HospitalSurvey
{
    public string PatientEmail { get; set; }
    public int ServiceGrade { get; set; }
    public int HygieneGrade { get; set; }
    public int OverallGrade { get; set; }
    public bool Recommendation { get; set; }
    public string Comment { get; set; } 

    [JsonConstructor]
    public HospitalSurvey(string patientEmail, int serviceGrade, int hygieneGrade, int overallGrade, bool recommendation, string comment)
    {
        PatientEmail = patientEmail;
        ServiceGrade = serviceGrade;
        HygieneGrade = hygieneGrade;
        OverallGrade = overallGrade;
        Recommendation = recommendation;
        Comment = comment;
    }

    public HospitalSurvey(HospitalSurveyDTO survayDto)
    {
        PatientEmail = survayDto.PatientEmail;
        ServiceGrade = survayDto.ServiceGrade;
        HygieneGrade = survayDto.HygieneGrade;
        OverallGrade = survayDto.OverallGrade;
        Recommendation = survayDto.Recommendation;
        Comment = survayDto.Comment;
    }
}