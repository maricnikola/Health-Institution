using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.User;

public class DirectorRepository : ISerializable
{
    private Director? _director;

    public Director? Director => _director;
    private readonly string _fileName = @".\..\..\..\Data\directors.json";
    
    

    public DirectorRepository()
    {
        Serializer.Load(this);
    } 
    public DirectorRepository(Director director)
    {
        _director = director;
        Serializer.Load(this);
    }
    


    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        var list = new List<object>();
        if (_director != null) list.Add(_director);
        return list;
    }

    public void Import(JToken token)
    {
        _director = token.ToObject<Director>();
    }
}