using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Survays;
using ZdravoCorp.Core.Repositories.SurvaysRepo;

namespace ZdravoCorp.Core.Services.ServayServices;

public class SurvayService : ISurvayService
{
    private readonly IDoctorSurvayRepository _doctorSurvayRepository;

    public SurvayService(IDoctorSurvayRepository doctorSurvayRepository)
    {
        _doctorSurvayRepository = doctorSurvayRepository;
    }

    public List<DoctorSurvay>? GetAllDoctorSurvays()
    {
        return _doctorSurvayRepository.GetAll() as List<DoctorSurvay>;
    }

    public DoctorSurvay? FindExistingDoctorSurvay(string doctorEmail, string patientEmail)
    {
        return GetAllDoctorSurvays().FirstOrDefault(survay => survay.DoctorEmail == doctorEmail && survay.PatientEmail == patientEmail);
    }

    public void AddDoctorSuvay(DoctorSurvayDTO doctorSurvay)
    {
        var survay = FindExistingDoctorSurvay(doctorSurvay.DoctorEmail, doctorSurvay.PatientEmail);
        if (survay != null)
        {
            _doctorSurvayRepository.Delete(survay);
            _doctorSurvayRepository.Insert(new DoctorSurvay(doctorSurvay));
        }
        else 
            _doctorSurvayRepository.Insert(new DoctorSurvay(doctorSurvay));

    }

    public List<DoctorSurvay> FindSurvaysForDoctor(string doctorEmail)
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
}