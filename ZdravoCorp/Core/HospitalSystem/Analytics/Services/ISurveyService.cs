using System.Collections.Generic;
using ZdravoCorp.Core.HospitalSystem.Analytics.Models;

namespace ZdravoCorp.Core.HospitalSystem.Analytics.Services;

public interface ISurveyService
{
    public IEnumerable<DoctorSurvey>? GetAllDoctorSurveys();
    public IEnumerable<HospitalSurvey>? GetAllHospitalSurveys();
    public DoctorSurvey? FindExistingDoctorSurvey(string doctorEmail, string patientEmail);
    public void AddDoctorSurvey(DoctorSurveyDTO doctorSurvey);
    public void AddHospitalSurvey(HospitalSurveyDTO hospitalSurvey);
    public List<DoctorSurvey> FindSurveysForDoctor(string doctorEmail);
    public double FindAverageGradeForDoctor(string doctorEmail);
    public HospitalSurvey? FindHospitalSurveyForPatient(string patientEmail);
    public int[] GetHospitalServiceGrades();
    public int[] GetHospitalHygieneGrades();
    public int[] GetHospitalOverallGrades();
    public int[] GetGradesForDoctor(string doctorEmail);
    public Dictionary<string, double> GetAllDoctorsWithGrades();
   



}