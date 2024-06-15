using System;
using ZdravoCorp.Core.HospitalSystem.Users.Models;

namespace ZdravoCorp.Core.HospitalSystem.Users.Repositories;

public interface IPatientRepository : IUserRepository<Patient>
{
    void UpdateNotificationTime(string email, TimeSpan newTimeSpan);
}