using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Core.Utilities;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Medicaments;
using ZdravoCorp.Core.Models.SpecialistsRefferals;

namespace ZdravoCorp.Core.Repositories.MedicamentsRepo;

public class MedicamentRepository : ISerializable,IMedicamentRepository
{
    private readonly string _fileName = @".\..\..\..\Data\medicaments.json";
    private List<Medicament>? _medicaments;

    public MedicamentRepository()
    {
        _medicaments = new List<Medicament>();
        Serializer.Load(this);
    }

    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _medicaments;
    }

    public void Import(JToken token)
    {
        _medicaments = token.ToObject<List<Medicament>>();
    }
    public void Insert(Medicament newMedicament)
    {
        _medicaments.Add(newMedicament);
        Serializer.Save(this);
    }
    public void SaveChanges()
    {
        Serializer.Save(this);
    }
    public List<Medicament> GetAll()
    {
        return _medicaments;
    }
    public void Delete(Medicament entity)
    {
        _medicaments.Remove(entity);
        Serializer.Save(this);
    }

    public Medicament GetById(string name)
    {
        return _medicaments.FirstOrDefault(med => med.Name == name);
    }

}
