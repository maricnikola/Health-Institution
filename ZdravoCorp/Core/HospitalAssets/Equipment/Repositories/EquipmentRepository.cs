using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalAssets.Equipment.Repositories;

public class EquipmentRepository : ISerializable, IEquipmentRepository
{
    private readonly string _fileName = @".\..\..\..\..\Data\equipment.json";
    private List<Models.Equipment>? _equipment;

    public EquipmentRepository()

    {
        _equipment = new List<Models.Equipment>();
        Serializer.Load(this);
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
        _equipment = token.ToObject<List<Models.Equipment>>();
    }


    public IEnumerable<Models.Equipment> GetAll()
    {
        return _equipment;
    }

    public void Insert(Models.Equipment newEquipment)
    {
        _equipment?.Add(newEquipment);
        Serializer.Save(this);
    }

    public void Delete(Models.Equipment entity)
    {
        _equipment.Remove(entity);
        Serializer.Save(this);
    }

    public Models.Equipment? GetById(int id)
    {
        return _equipment.FirstOrDefault(eq => eq.Id == id);
    }
}