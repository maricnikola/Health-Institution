using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.Equipment;

public class EquipmentRepository : ISerializable
{
    private List<Models.Equipment.Equipment>? _equipment;
    private readonly string _fileName = @".\..\..\..\Data\equipment.json";

    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public EquipmentRepository()

    {
        _equipment = new List<Models.Equipment.Equipment>();
        Serializer.Load(this);
    }


    public void Add(Models.Equipment.Equipment newEquipment)
    {
        _equipment.Add(newEquipment);
    }

    public Models.Equipment.Equipment? GetById(int id)
    {
        return _equipment.FirstOrDefault(eq => eq.Id == id);
    }

    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _equipment;
    }

    public void Import(JToken token)
    {
        _equipment = token.ToObject<List<Models.Equipment.Equipment>>();
    }
}