using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.Repositories.UsersRepo;

public interface IUserRepository
{
    void AddUser(User user);
    User? GetUserByEmail(string email);
    bool ValidateEmail(string email);
}