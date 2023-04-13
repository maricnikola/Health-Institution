using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Core.Models.User;

namespace ZdravoCorp.Core.Repositories.User;

public class NurseRepository
{
    
    private List<Nurse> _nurses;
    private readonly string _fileName = @".\..\..\..\Data\nurses.json";
    
    
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public NurseRepository()
    {
        _nurses = new List<Nurse>();
        LoadFromFile();
    }

    public void AddNurse(Nurse nurse)
    {
        _nurses.Add(nurse);
    }
    public void SaveToFile()
    {
        var usersForFile = ReduceForSerialization();
        var nurses = JsonSerializer.Serialize(usersForFile, _serializerOptions);
        File.WriteAllText(this._fileName, nurses);
    }
    public void LoadFromFile()
    {
        var text = File.ReadAllText(_fileName);
        var nurses = JsonSerializer.Deserialize<List<Nurse>>(text);
        nurses.ForEach(nurse => _nurses.Add(nurse));
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