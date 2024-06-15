using ZdravoCorp.Core.HospitalSystem.Users.Models;

namespace ZdravoCorp.Core.HospitalSystem.Users.Repositories;

public interface IDoctorRepository : IUserRepository<Doctor>
{
    void UpdateGrade(string email, double grade);
}