using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Models.User;

namespace ZdravoCorp.Core.Repositories.User;

public class NurseRepository
{
    
    private readonly List<Nurse?> _nurses;
    private readonly string _fileName = @".\..\..\..\Data\nurses.json";
    
    
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public NurseRepository()
    {
        _nurses = new List<Nurse?>();
        LoadFromFile();
    }

    public void Add(Nurse? nurse)
    {
        _nurses.Add(nurse);
    }
    public void SaveToFile()
    {
        if (_nurses.Count == 0)
        {
            Trace.WriteLine($"Repository is empty! {this.GetType()}");
            return;
        }
        var usersForFile = ReduceForSerialization();
        var nurses = JsonSerializer.Serialize(usersForFile, _serializerOptions);
        File.WriteAllText(this._fileName, nurses);
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

            List<Nurse?>? nurses = JsonSerializer.Deserialize<List<Nurse>>(text);
            nurses?.ForEach(nurse => _nurses.Add(nurse));
        }
        catch (JsonException e)
        {
            Trace.WriteLine(e);
            throw;
        }
    }
    public Nurse? GetNurseByEmail(string email)
    {
        return _nurses.FirstOrDefault(nurse => nurse.Email == email);
    }
    
    private List<dynamic> ReduceForSerialization()
    {
        return this._nurses.Select(user => user.GetNurseForSerialization()).ToList();
    }
    
    

}