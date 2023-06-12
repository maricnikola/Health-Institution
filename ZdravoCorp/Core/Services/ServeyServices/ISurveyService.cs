using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Survays;


namespace ZdravoCorp.Core.Services.ServayServices;

public interface ISurveyService
{
    public IEnumerable<DoctorSurvey>? GetAllDoctorSurvays();
    public IEnumerable<HospitalSurvey>? GetAllHospitalSurvays();
    public DoctorSurvey? FindExistingDoctorSurvay(string doctorEmail, string patientEmail);
    public void AddDoctorSuvay(DoctorSurveyDTO doctorSurvay);
    public void AddHospitalSurvay(HospitalSurveyDTO hospitalSurvay);
    public List<DoctorSurvey> FindSurvaysForDoctor(string doctorEmail);
    public double FindAverageGradeForDoctor(string doctorEmail);
    public HospitalSurvey? FindHospitalSurvayForPatient(string patientEmail);


}