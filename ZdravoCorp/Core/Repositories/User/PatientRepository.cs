using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Models.User;

namespace ZdravoCorp.Core.Repositories.User;

public class PatientRepository
{
        
    private readonly List<Patient> _patients;
    private readonly string _fileName = @".\..\..\..\Data\patients.json";
    
    
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public PatientRepository()
    {
        _patients = new List<Patient>();
        LoadFromFile();
    }

    public void Add(Patient patient)
    {
        _patients.Add(patient);
    }
    public void SaveToFile()
    {
        if (_patients.Count == 0)
        {
            Trace.WriteLine($"Repository is empty! {this.GetType()}");
            return;
        }
        var usersForFile = ReduceForSerialization();
        var patients = JsonSerializer.Serialize(usersForFile, _serializerOptions);
        File.WriteAllText(this._fileName, patients);
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

            var patients = JsonSerializer.Deserialize<List<Patient>>(text);
            patients.ForEach(patient => _patients.Add(patient));
        }
        catch (JsonException e)
        {
            Trace.WriteLine(e);
            throw;
        }
    }
    public Patient? GetPatientByEmail(string email)
    {
        return _patients.FirstOrDefault(patient => patient.Email == email);
    }
    
    private List<dynamic> ReduceForSerialization()
    {
        return this._patients.Select(user => user.GetPatientForSerialization()).ToList();
    }
}