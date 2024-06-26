﻿using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalSystem.Users.Repositories;

public class NurseRepository : ISerializable, INurseRepository
{
    private readonly string _fileName = @".\..\..\..\..\Data\nurses.json";
    private List<Nurse?> _nurses;


    public NurseRepository()
    {
        _nurses = new List<Nurse?>();
        Serializer.Load(this);
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

    public void Add(Nurse? nurse)
    {
        _nurses.Add(nurse);
    }

    public IEnumerable<Nurse> GetAll()
    {
        return _nurses;
    }

    public void Insert(Nurse entity)
    {
        _nurses.Add(entity);
    }

    public Nurse? GetByEmail(string email)
    {
        return _nurses.FirstOrDefault(nurse => nurse.Email == email);
    }
}