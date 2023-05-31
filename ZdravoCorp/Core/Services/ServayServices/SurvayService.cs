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

    public DoctorSurvay? FindExistingDoctorSurvay(string doctorEmail, string patientEmail)
    {
        return _doctorSurvayRepository.GetAll().FirstOrDefault(survay => survay.DoctorEmail == doctorEmail && survay.PatientEmail == patientEmail);
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
}