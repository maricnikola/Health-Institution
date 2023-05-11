﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Documents;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.User;

public class DoctorRepository : ISerializable
{
    private List<Doctor>? _doctors;
    private readonly string _fileName = @".\..\..\..\Data\doctors.json";
    public List<Doctor>? Doctors => _doctors;


    public DoctorRepository()
    {
        _doctors = new List<Doctor>();
        Serializer.Load(this);
    }


    public Doctor? GetDoctorByEmail(string email)
    {
        return _doctors.FirstOrDefault(doctor => doctor.Email == email);
    }

    public List<Doctor> GetAll()
    {
        return _doctors;
    }

    public List<Doctor> GetAllWithCertainSpecialization(Doctor.SpecializationType specialization)
    {
        List<Doctor> wantedDoctors = new List<Doctor>();
        foreach (var doctor in Doctors)
        {
            if (doctor.Specialization == specialization)
                wantedDoctors.Add(doctor);
        }
        return wantedDoctors;
    }

    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _doctors;
    }

    public void Import(JToken token)
    {
        _doctors = token.ToObject<List<Doctor>>();
    }
}