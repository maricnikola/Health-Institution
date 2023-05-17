using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.Repositories.UsersRepo;

public interface IUserRepository
{
    void Insert(User user);
    User? GetByEmail(string email);
    bool ValidateEmail(string email);
}