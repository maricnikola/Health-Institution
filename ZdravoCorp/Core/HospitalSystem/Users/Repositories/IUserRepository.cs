using System.Collections.Generic;

namespace ZdravoCorp.Core.HospitalSystem.Users.Repositories;

public interface IUserRepository<T>where T:class
{
    IEnumerable<T> GetAll();
    void Insert(T entity);
    T? GetByEmail(string email);
}