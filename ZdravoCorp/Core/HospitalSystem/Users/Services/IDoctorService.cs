using System.Collections.Generic;
using ZdravoCorp.Core.HospitalSystem.Users.Models;

namespace ZdravoCorp.Core.HospitalSystem.Users.Services;

public interface IDoctorService
{
    public List<Doctor>? GetAll();
    public Doctor? GetByEmail(string email);
    public void AddDoctor(DoctorDTO doctor);
    public void Update(string email, DoctorDTO doctorDto);
    public void Delete(string email);
    public List<Doctor> GetAllWithCertainSpecialization(Doctor.SpecializationType specialization);
    public List<Doctor> GetAllSpecialized(Doctor.SpecializationType specializationType);
    public void UpdateGrade(string email, double grade);

}