using System.Collections.Generic;
using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.Repositories.UsersRepo;

public interface IUserRepository<T>where T:class
{
    IEnumerable<T> GetAll();
    void Insert(T entity);
    T? GetByEmail(string email);
}