using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Survays;


namespace ZdravoCorp.Core.Services.ServayServices;

public interface ISurvayService
{
    public IEnumerable<DoctorSurvay>? GetAllDoctorSurvays();
    public IEnumerable<HospitalSurvay>? GetAllHospitalSurvays();
    public DoctorSurvay? FindExistingDoctorSurvay(string doctorEmail, string patientEmail);
    public void AddDoctorSuvay(DoctorSurvayDTO doctorSurvay);
    public void AddHospitalSurvay(HospitalSurvayDTO hospitalSurvay);
    public List<DoctorSurvay> FindSurvaysForDoctor(string doctorEmail);
    public double FindAverageGradeForDoctor(string doctorEmail);
    public HospitalSurvay? FindHospitalSurvayForPatient(string patientEmail);


}