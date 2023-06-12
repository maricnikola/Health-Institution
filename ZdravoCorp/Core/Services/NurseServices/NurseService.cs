using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.UsersRepo;

namespace ZdravoCorp.Core.Services.NurseServices;

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