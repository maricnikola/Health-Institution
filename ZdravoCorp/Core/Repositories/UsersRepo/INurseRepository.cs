using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.Repositories.UsersRepo;

public interface INurseRepository
{
    void Add(Nurse? nurse);
    Nurse? GetByEmail(string email);
}