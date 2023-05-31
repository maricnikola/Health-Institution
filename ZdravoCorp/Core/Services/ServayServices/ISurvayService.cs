using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Survays;


namespace ZdravoCorp.Core.Services.ServayServices;

public interface ISurvayService
{
    public List<DoctorSurvay>? GetAllDoctorSurvays();
    public DoctorSurvay? FindExistingDoctorSurvay(string doctorEmail, string patientEmail);
    public void AddDoctorSuvay(DoctorSurvayDTO doctorSurvay);
    public List<DoctorSurvay> FindSurvaysForDoctor(string doctorEmail);
    public double FindAverageGradeForDoctor(string doctorEmail);

}