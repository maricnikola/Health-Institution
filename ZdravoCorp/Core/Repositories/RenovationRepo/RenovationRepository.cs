using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Renovation;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.RenovationRepo;

public class RenovationRepository : IRenovationRepository, ISerializable
{
    private readonly string _fileName = @".\..\..\..\Data\renovations.json";
    private List<Renovation>? _renovations;


    public RenovationRepository()
    {
        _renovations = new List<Renovation>();
         //Serializer.Load(this);
    }
    public IEnumerable<Renovation> GetAll()
    {
        return _renovations;
    }

    public void Insert(Renovation entity)
    {
        _renovations.Add(entity);
        Serializer.Save(this);
    }

    public void Delete(Renovation entity)
    {
        _renovations.Remove(entity);
        Serializer.Save(this);
    }

    public Renovation GetById(int id)
    {
        return _renovations.FirstOrDefault(renovation => renovation.Id == id);
    }

    public void UpdateStatus(int id, Renovation.RenovationStatus status)
    {
        GetById(id).Status = status;
        Serializer.Save(this);
    }

    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _renovations;
    }

    public void Import(JToken token)
    {
        _renovations = token.ToObject<List<Renovation>>();
    }
}