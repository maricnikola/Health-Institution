using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Core.Exceptions;

namespace ZdravoCorp.Core.Repositories.Equipment;

public class EquipmentRepository
{
    private readonly List<Models.Equipment.Equipment>  _equipment;
    private readonly string _fileName =  @".\..\..\..\Data\equipment.json";
    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public EquipmentRepository()

    {
        _equipment = new List<Models.Equipment.Equipment>();
        LoadFromFile();
    }

    public void SaveToFile()
    {
        if (_equipment.Count == 0)
        {
            Trace.WriteLine($"Repository is empty! {this.GetType()}");
            return;
        }
        var equipment= JsonSerializer.Serialize(_equipment, _serializerOptions);
        File.WriteAllText(this._fileName, equipment);
    }
    public void LoadFromFile()
    {
        var text = File.ReadAllText(_fileName);
        if (text == "")
            throw new EmptyFileException("File is empty!");
        try
        {
            var equipment = JsonSerializer.Deserialize<List<Models.Equipment.Equipment>>(text);
            equipment?.ForEach(eq => _equipment.Add(eq));
        }
        catch (JsonException jsonException)
        {
           Trace.WriteLine(jsonException);
        }
    }

    public void Add(Models.Equipment.Equipment newEquipment)
    {
        _equipment.Add(newEquipment);
    }

    public Models.Equipment.Equipment? GetById(int id)
    {
        return _equipment.FirstOrDefault(eq => eq.Id == id);
    }
}