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
    private readonly IHospitalSurvayRepository _hospitalSurvayRepository;

    public SurvayService(IDoctorSurvayRepository doctorSurvayRepository, IHospitalSurvayRepository hospitalSurvayRepository)
    {
        _doctorSurvayRepository = doctorSurvayRepository;
        _hospitalSurvayRepository = hospitalSurvayRepository;
    }

    public IEnumerable<DoctorSurvay>? GetAllDoctorSurvays()
    {
        return _doctorSurvayRepository.GetAll() as List<DoctorSurvay>;
    }

    public IEnumerable<HospitalSurvay>? GetAllHospitalSurvays()
    {
        return _hospitalSurvayRepository.GetAll() as List<HospitalSurvay>;
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

    public void AddHospitalSurvay(HospitalSurvayDTO hospitalSurvay)
    {
        var survay = FindHospitalSurvayForPatient(hospitalSurvay.PatientEmail);
        if (survay != null)
        {
            _hospitalSurvayRepository.Delete(survay);
            _hospitalSurvayRepository.Insert(new HospitalSurvay(hospitalSurvay));
        }
        else
            _hospitalSurvayRepository.Insert(new HospitalSurvay(hospitalSurvay));
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

    public HospitalSurvay? FindHospitalSurvayForPatient(string patientEmail)
    {
        return GetAllHospitalSurvays()!.FirstOrDefault(survay => survay.PatientEmail == patientEmail);
    }
}