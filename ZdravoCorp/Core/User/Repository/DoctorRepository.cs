﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZdravoCorp.Core.Appointments.Entities;

namespace ZdravoCorp.Core.User.Repository;

public class DoctorRepository
{

    private List<Doctor> _doctors;
    private readonly string _fileName = @".\..\..\..\Data\doctors.json";
    
    
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    };

    public DoctorRepository()
    {
        _doctors = new List<Doctor>();
        LoadFromFile();
    }

    public void SaveToFile()
    {
        var doctors = JsonSerializer.Serialize(this._doctors, _serializerOptions);
        File.WriteAllText(this._fileName, doctors);
    }
    public void LoadFromFile()
    {
        var text = File.ReadAllText(_fileName);
        var doctors = JsonSerializer.Deserialize<List<Doctor>>(text);
        doctors.ForEach(doctor => _doctors.Add(doctor));
    }
    public Doctor? GetDoctorByEmail(string email)
    {
        return _doctors.FirstOrDefault(doctor => doctor.Email == email);
    }
    
    private List<dynamic> ReduceForSerialization()
    {
        return this._doctors.Select(user => user.GetDoctorForSerialization()).ToList();
    }
    
}
