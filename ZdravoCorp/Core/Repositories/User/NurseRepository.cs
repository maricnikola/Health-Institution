using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.User;

public class NurseRepository : ISerializable
{
    
    private  List<Nurse?> _nurses;
    private readonly string _fileName = @".\..\..\..\Data\nurses.json";
    
    

    public NurseRepository()
    {
        _nurses = new List<Nurse?>();
        Serializer.Load(this);
    }

    public void Add(Nurse? nurse)
    {
        _nurses.Add(nurse);
    }
    public Nurse? GetNurseByEmail(string email)
    {
        return _nurses.FirstOrDefault(nurse => nurse.Email == email);
    }
    



    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _nurses;
    }

    public void Import(JToken token)
    {
        _nurses = token.ToObject<List<Nurse>>();
    }
}