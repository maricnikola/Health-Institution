using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Models.User;

namespace ZdravoCorp.Core.Repositories.User;

public class DirectorRepository
{
    private Director? _director;

    public Director? Director => _director;
    private readonly string _fileName = @".\..\..\..\Data\directors.json";
    
    
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public DirectorRepository()
    {
        LoadFromFile();
    } 
    public DirectorRepository(Director director)
    {
        _director = director;
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
        if (text == "")
            throw new EmptyFileException("File is empty!");
        try
        {
            _director = JsonSerializer.Deserialize<Director>(text);
        }
        catch (JsonException e)
        {
            
            Trace.WriteLine(e);
        }
    }
    


}