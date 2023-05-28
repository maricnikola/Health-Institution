using System;
using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.Repositories.UsersRepo;

public interface IPatientRepository : IUserRepository<Patient>
{
    void UpdateNotificationTime(string email, TimeSpan newTimeSpan);
}