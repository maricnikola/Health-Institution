using System.Collections.Generic;
using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.Services.UserServices;

public interface IUserService
{
    void AddUser(UserDTO userDto);
    User? GetByEmail(string email);
    bool ValidateEmail(string email);
    public List<Doctor> GetAllWithCertainSpecialization(Doctor.SpecializationType specialization);
    public List<Doctor> GetAllSpecialized(Doctor.SpecializationType specializationType);

}