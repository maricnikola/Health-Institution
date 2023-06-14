using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.PatientFiles.Refferals.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.PatientFiles.Refferals.Repositories;

public class SpecialistsRefferalRepository: ISerializable, ISpecialistsRefferalRepository
{
    private readonly string _fileName = @".\..\..\..\..\Data\specialistsRefferal.json";
    private List<SpecialistsRefferal>? _specialistsRefferals;

    public SpecialistsRefferalRepository()
    {
        _specialistsRefferals = new List<SpecialistsRefferal>();
        Serializer.Load(this);
    }

    public void Insert(SpecialistsRefferal newSpecialistsRefferal)
    {
        _specialistsRefferals.Add(newSpecialistsRefferal);

        Serializer.Save(this);
    }

    string ISerializable.FileName()
    {
        return _fileName;
    }

    IEnumerable<object>? ISerializable.GetList()
    {
        return _specialistsRefferals;
    }

    void ISerializable.Import(JToken token)
    {
        _specialistsRefferals = token.ToObject<List<SpecialistsRefferal>>();
    }
    public void SaveChanges()
    {
        Serializer.Save(this);
    }

    public List<SpecialistsRefferal> GetAll()
    {
        return _specialistsRefferals;
    }

    public void Delete(SpecialistsRefferal entity)
    {
        _specialistsRefferals.Remove(entity);
        Serializer.Save(this);
    }

    public SpecialistsRefferal GetById(int id)
    {
        return _specialistsRefferals.FirstOrDefault(refferal => refferal.Id == id);
    }

}
