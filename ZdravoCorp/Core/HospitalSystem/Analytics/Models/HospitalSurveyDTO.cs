namespace ZdravoCorp.Core.HospitalSystem.Analytics.Models;

public class HospitalSurveyDTO
{
    public string PatientEmail { get; set; }
    public int ServiceGrade { get; set; }
    public int HygieneGrade { get; set; }
    public int OverallGrade { get; set; }
    public bool Recommendation { get; set; }
    public string Comment { get; set; }

    public HospitalSurveyDTO(string patientEmail, int serviceGrade, int hygieneGrade, int overallGrade, bool recommendation, string comment)
    {
        PatientEmail = patientEmail;
        ServiceGrade = serviceGrade;
        HygieneGrade = hygieneGrade;
        OverallGrade = overallGrade;
        Recommendation = recommendation;
        Comment = comment;
    }
}