using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Core.HospitalSystem.Analytics.Models;
using ZdravoCorp.Core.HospitalSystem.Analytics.Repositories;
using ZdravoCorp.Core.HospitalSystem.Users.Services;

namespace ZdravoCorp.Core.HospitalSystem.Analytics.Services;

public class SurveyService : ISurveyService
{
    private readonly IDoctorSurveyRepository _doctorSurveyRepository;
    private readonly IHospitalSurveyRepository _hospitalSurveyRepository;
    private readonly IDoctorService _doctorService;

    public SurveyService(IDoctorSurveyRepository doctorSurveyRepository, IHospitalSurveyRepository hospitalSurveyRepository, IDoctorService doctorService)
    {
        _doctorSurveyRepository = doctorSurveyRepository;
        _hospitalSurveyRepository = hospitalSurveyRepository;
        _doctorService = doctorService;
    }

    public IEnumerable<DoctorSurvey>? GetAllDoctorSurveys()
    {
        return _doctorSurveyRepository.GetAll() as List<DoctorSurvey>;
    }

    public IEnumerable<HospitalSurvey>? GetAllHospitalSurveys()
    {
        return _hospitalSurveyRepository.GetAll() as List<HospitalSurvey>;
    }

    public DoctorSurvey? FindExistingDoctorSurvey(string doctorEmail, string patientEmail)
    {
        return GetAllDoctorSurveys().FirstOrDefault(survey => survey.DoctorEmail == doctorEmail && survey.PatientEmail == patientEmail);
    }

    public void AddDoctorSurvey(DoctorSurveyDTO doctorSurvey)
    {
        var survey = FindExistingDoctorSurvey(doctorSurvey.DoctorEmail, doctorSurvey.PatientEmail);
        if (survey != null)
        {
            _doctorSurveyRepository.Delete(survey);
            _doctorSurveyRepository.Insert(new DoctorSurvey(doctorSurvey));
        }
        else 
            _doctorSurveyRepository.Insert(new DoctorSurvey(doctorSurvey));

    }

    public void AddHospitalSurvey(HospitalSurveyDTO hospitalSurvey)
    {
        var survey = FindHospitalSurveyForPatient(hospitalSurvey.PatientEmail);
        if (survey != null)
        {
            _hospitalSurveyRepository.Delete(survey);
            _hospitalSurveyRepository.Insert(new HospitalSurvey(hospitalSurvey));
        }
        else
            _hospitalSurveyRepository.Insert(new HospitalSurvey(hospitalSurvey));
    }

    public List<DoctorSurvey> FindSurveysForDoctor(string doctorEmail)
    {
        return GetAllDoctorSurveys().Where(survey => survey.DoctorEmail == doctorEmail).ToList();
    }

    public double FindAverageGradeForDoctor(string doctorEmail)
    {
        var doctorSurveys = FindSurveysForDoctor(doctorEmail);
        double count = doctorSurveys.Count;
        if (count == 0)
            return 0;
        double sum = doctorSurveys.Sum(survey => survey.Grade);
        double avg = sum / count;
        return avg;
    }

    public HospitalSurvey? FindHospitalSurveyForPatient(string patientEmail)
    {
        return GetAllHospitalSurveys()!.FirstOrDefault(survey => survey.PatientEmail == patientEmail);
    }

    public int[] GetHospitalServiceGrades()
    {
        int[] grades = { 0, 0, 0, 0, 0 };
        foreach (var survey in _hospitalSurveyRepository.GetAll())
        {
            grades[survey.ServiceGrade - 1]++;
        }

        return grades;
    }

    public int[] GetHospitalHygieneGrades()
    {
        int[] grades = { 0, 0, 0, 0, 0 };
        foreach (var survey in _hospitalSurveyRepository.GetAll())
        {
            grades[survey.HygieneGrade - 1]++;
        }

        return grades;
    }

    public int[] GetHospitalOverallGrades()
    {
        int[] grades = { 0, 0, 0, 0, 0 };
        foreach (var survey in _hospitalSurveyRepository.GetAll())
        {
            grades[survey.OverallGrade - 1]++;
        }

        return grades;
    }

    public int[] GetGradesForDoctor(string doctorEmail)
    {
        int[] grades = { 0, 0, 0, 0, 0 };
        foreach (var survey in FindSurveysForDoctor(doctorEmail))
        {
            grades[survey.Grade - 1]++;
        }

        return grades;
    }

    public Dictionary<string, double> GetAllDoctorsWithGrades()
    {
        Dictionary<string, double> grades = new Dictionary<string, double>();
        foreach (var doctor in _doctorService.GetAll()!)
        {
            var grade = FindAverageGradeForDoctor(doctor.Email);
            if (grade > 0)
                grades[doctor.FullName] = grade;
        }

        return grades;
    }
}