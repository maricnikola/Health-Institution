using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalSystem.Users.Repositories;

public class DirectorRepository : ISerializable, IDirectorRepository
{
    private readonly string _fileName = @".\..\..\..\..\Data\directors.json";


    public DirectorRepository()
    {
        Serializer.Load(this);
    }

    public DirectorRepository(Director director)
    {
        Director = director;
        Serializer.Load(this);
    }

    public Director? Director { get; private set; }


    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        var list = new List<object>();
        if (Director != null) list.Add(Director);
        return list;
    }

    public void Import(JToken token)
    {
        Director = token.ToObject<Director>();
    }

    public IEnumerable<Director> GetAll()
    {
        throw new System.NotImplementedException();
    }

    public void Insert(Director entity)
    {
        Director = entity;
    }

    public Director? GetByEmail(string email)
    {
        if (Director?.Email == email)
            return Director;
        return null;
    }
}