using ZdravoCorp.Core.HospitalSystem.Users.Models;

namespace ZdravoCorp.Core.HospitalSystem.Users.Services;

public interface INurseService
{
    public Nurse? GetByEmail(string email);
}