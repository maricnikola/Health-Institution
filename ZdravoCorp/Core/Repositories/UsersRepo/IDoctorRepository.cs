using System.Collections.Generic;
using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.Repositories.UsersRepo;

public interface IDoctorRepository
{
    Doctor? GetDoctorByEmail(string email);
    List<Doctor> GetAll();
    List<Doctor> GetAllWithCertainSpecialization(Doctor.SpecializationType specialization);
    List<Doctor> GetAllSpecialized(Doctor.SpecializationType specializationType);
}