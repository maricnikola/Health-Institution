using Newtonsoft.Json;

namespace ZdravoCorp.Core.HospitalSystem.Analytics.Models;

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

    public HospitalSurvey(HospitalSurveyDTO surveyDto)
    {
        PatientEmail = surveyDto.PatientEmail;
        ServiceGrade = surveyDto.ServiceGrade;
        HygieneGrade = surveyDto.HygieneGrade;
        OverallGrade = surveyDto.OverallGrade;
        Recommendation = surveyDto.Recommendation;
        Comment = surveyDto.Comment;
    }
}