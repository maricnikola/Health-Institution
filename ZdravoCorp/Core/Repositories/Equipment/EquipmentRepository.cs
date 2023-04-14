using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ZdravoCorp.Core.Repositories.Equipment;

public class EquipmentRepository
{
    private List<Models.Equipment.Equipment>  _equipment;
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
         var equipment= JsonSerializer.Serialize(_equipment, _serializerOptions);
        File.WriteAllText(this._fileName, equipment);
    }
    public void LoadFromFile()
    {
        var text = File.ReadAllText(_fileName);
        if (text == "")
            return;
        var equipment = JsonSerializer.Deserialize<List<Models.Equipment.Equipment>>(text);
        equipment.ForEach(equipment => _equipment.Add(equipment));
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