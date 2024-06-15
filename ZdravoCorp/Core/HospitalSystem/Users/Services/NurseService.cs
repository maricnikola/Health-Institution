using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Repositories;

namespace ZdravoCorp.Core.HospitalSystem.Users.Services;

public class NurseService : INurseService
{
    private INurseRepository _nurseRepository;

    public NurseService(INurseRepository nurseRepository)
    {
        _nurseRepository = nurseRepository;
    }
    public Nurse? GetByEmail(string email)
    {
        return _nurseRepository.GetByEmail(email);
    }
}