using System.IO;
using System.Text.Json;
using ZdravoCorp.Core.Models.User;

namespace ZdravoCorp.Core.Repositories.User;

public class DirectorRepository
{
    public Director _director { get; set; }
    private readonly string _fileName = @".\..\..\..\Data\directors.json";
    
    
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public DirectorRepository()
    {
        LoadFromFile();
    }
    
    public void SaveToFile()
    {
        var director = JsonSerializer.Serialize(_director, _serializerOptions);
        
        File.WriteAllText(this._fileName, director);
    }
    public void LoadFromFile()
    {
        var text = File.ReadAllText(_fileName);
        _director = JsonSerializer.Deserialize<Director>(text);
    }
    


}