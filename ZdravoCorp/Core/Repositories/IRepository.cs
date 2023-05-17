using System.Collections;
using System.Collections.Generic;

namespace ZdravoCorp.Core.Repositories;

public interface IRepository<T>where T:class
{
    IEnumerable<T> GetAll();
    void Insert(T entity);
    void Delete(T entity);
    T GetById(int id);
}