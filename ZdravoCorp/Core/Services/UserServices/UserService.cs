using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.UsersRepo;

namespace ZdravoCorp.Core.Services.UserServices;

public class UserService : IUserService
{
    private IUserRepository<User> _userRepository;
    private IDoctorRepository _doctorRepository;
    public UserService(IUserRepository<User> userRepository, IDoctorRepository doctorRepository)
    {
        _userRepository = userRepository;
        _doctorRepository = doctorRepository;

    }
    
    public void AddUser(UserDTO userDto)
    {
        _userRepository.Insert(new User(userDto));
    }

    public User? GetByEmail(string email)
    {
        return _userRepository.GetByEmail(email);
    }

    public bool ValidateEmail(string email)
    {
        return _userRepository.GetAll().FirstOrDefault(user => user.Email == email) != null;
    }
    
}