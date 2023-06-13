using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Equipments;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.EquipmentRepo;

public class EquipmentRepository : ISerializable, IEquipmentRepository
{
    private readonly string _fileName = @".\..\..\..\..\Data\equipment.json";
    private List<Equipment>? _equipment;

    public EquipmentRepository()

    {
        _equipment = new List<Equipment>();
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
        _equipment = token.ToObject<List<Equipment>>();
    }


    public IEnumerable<Equipment> GetAll()
    {
        return _equipment;
    }

    public void Insert(Equipment newEquipment)
    {
        _equipment?.Add(newEquipment);
        Serializer.Save(this);
    }

    public void Delete(Equipment entity)
    {
        _equipment.Remove(entity);
        Serializer.Save(this);
    }

    public Equipment? GetById(int id)
    {
        return _equipment.FirstOrDefault(eq => eq.Id == id);
    }
}