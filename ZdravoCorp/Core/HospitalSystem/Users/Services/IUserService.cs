using ZdravoCorp.Core.HospitalSystem.Users.Models;

namespace ZdravoCorp.Core.HospitalSystem.Users.Services;

public interface IUserService
{
    void AddUser(UserDTO userDto);
    User? GetByEmail(string email);
    bool ValidateEmail(string email);
    

}