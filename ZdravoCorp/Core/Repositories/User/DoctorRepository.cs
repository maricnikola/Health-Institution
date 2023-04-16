﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Models.User;

namespace ZdravoCorp.Core.Repositories.User;

public class DoctorRepository
{

    private readonly List<Doctor> _doctors;
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
        if (_doctors.Count == 0)
        {
            Trace.WriteLine($"Repository is empty! {this.GetType()}");
            return;
        }
        var doctors = JsonSerializer.Serialize(this._doctors, _serializerOptions);
        File.WriteAllText(this._fileName, doctors);
    }
    public void LoadFromFile()
    {
        var text = File.ReadAllText(_fileName);
        if (text == "")
        {
            throw new EmptyFileException("File is empty!");
        }

        try
        {

            var doctors = JsonSerializer.Deserialize<List<Doctor>>(text);
            doctors?.ForEach(doctor => _doctors.Add(doctor));

        }
        catch (JsonException e)
        {
            Trace.WriteLine(e);

        }
    }
    public Doctor? GetDoctorByEmail(string email)
    {
        return _doctors.FirstOrDefault(doctor => doctor.Email == email);
    }
    
    private List<dynamic> ReduceForSerialization()
    {
        return this._doctors.Select(user => user.GetDoctorForSerialization()).ToList();
    }

    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object> Repository()
    {
        return _doctors;
    }

    public void Add(object? obj)
    {
       _doctors.Add((Doctor) obj);
    }

    public List<Doctor> GetAll()
    {
        return _doctors;
    }
}
