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

public class PatientRepository : ISerializable
{
    private List<Patient> _patients;
    private readonly string _fileName = @".\..\..\..\Data\patients.json";
    public List<Patient> Patients => _patients;


    public PatientRepository()
    {
        _patients = new List<Patient>();
        Serializer.Load(this);
    }

    public void Add(Patient patient)
    {
        _patients.Add(patient);
    }

    public Patient? GetPatientByEmail(string email)
    {
        return _patients.FirstOrDefault(patient => patient.Email == email);
    }


    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _patients;
    }

    public void Import(JToken token)
    {
        _patients = token.ToObject<List<Patient>>();
    }
}