using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Survays;
using ZdravoCorp.Core.Repositories.SurvaysRepo;

namespace ZdravoCorp.Core.Services.ServayServices;

public class SurveyService : ISurveyService
{
    private readonly IDoctorSurveyRepository _doctorSurvayRepository;
    private readonly IHospitalSurveyRepository _hospitalSurvayRepository;

    public SurveyService(IDoctorSurveyRepository doctorSurvayRepository, IHospitalSurveyRepository hospitalSurvayRepository)
    {
        _doctorSurvayRepository = doctorSurvayRepository;
        _hospitalSurvayRepository = hospitalSurvayRepository;
    }

    public IEnumerable<DoctorSurvey>? GetAllDoctorSurvays()
    {
        return _doctorSurvayRepository.GetAll() as List<DoctorSurvey>;
    }

    public IEnumerable<HospitalSurvey>? GetAllHospitalSurvays()
    {
        return _hospitalSurvayRepository.GetAll() as List<HospitalSurvey>;
    }

    public DoctorSurvey? FindExistingDoctorSurvay(string doctorEmail, string patientEmail)
    {
        return GetAllDoctorSurvays().FirstOrDefault(survay => survay.DoctorEmail == doctorEmail && survay.PatientEmail == patientEmail);
    }

    public void AddDoctorSuvay(DoctorSurveyDTO doctorSurvay)
    {
        var survay = FindExistingDoctorSurvay(doctorSurvay.DoctorEmail, doctorSurvay.PatientEmail);
        if (survay != null)
        {
            _doctorSurvayRepository.Delete(survay);
            _doctorSurvayRepository.Insert(new DoctorSurvey(doctorSurvay));
        }
        else 
            _doctorSurvayRepository.Insert(new DoctorSurvey(doctorSurvay));

    }

    public void AddHospitalSurvay(HospitalSurveyDTO hospitalSurvay)
    {
        var survay = FindHospitalSurvayForPatient(hospitalSurvay.PatientEmail);
        if (survay != null)
        {
            _hospitalSurvayRepository.Delete(survay);
            _hospitalSurvayRepository.Insert(new HospitalSurvey(hospitalSurvay));
        }
        else
            _hospitalSurvayRepository.Insert(new HospitalSurvey(hospitalSurvay));
    }

    public List<DoctorSurvey> FindSurvaysForDoctor(string doctorEmail)
    {
        return GetAllDoctorSurvays().Where(survay => survay.DoctorEmail == doctorEmail).ToList();
    }

    public double FindAverageGradeForDoctor(string doctorEmail)
    {
        var doctorsSturvays = FindSurvaysForDoctor(doctorEmail);
        double count = doctorsSturvays.Count;
        if (count == 0)
            return 5;
        double sum = doctorsSturvays.Sum(survay => survay.Grade);
        double avg = sum / count;
        return avg;
    }

    public HospitalSurvey? FindHospitalSurvayForPatient(string patientEmail)
    {
        return GetAllHospitalSurvays()!.FirstOrDefault(survay => survay.PatientEmail == patientEmail);
    }
}