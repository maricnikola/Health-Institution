using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Core.Models.User;

namespace ZdravoCorp.Core.Repositories.User;

public class PatientRepository
{
        
    private List<Patient> _patients;
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

    public void AddNurse(Patient patient)
    {
        _patients.Add(patient);
    }
    public void SaveToFile()
    {
        var usersForFile = ReduceForSerialization();
        var patients = JsonSerializer.Serialize(usersForFile, _serializerOptions);
        File.WriteAllText(this._fileName, patients);
    }
    public void LoadFromFile()
    {
        var text = File.ReadAllText(_fileName);
        var patients = JsonSerializer.Deserialize<List<Patient>>(text);
        patients.ForEach(patient => _patients.Add(patient));
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