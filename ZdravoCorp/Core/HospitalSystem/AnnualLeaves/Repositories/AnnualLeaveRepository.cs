using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Repositories;

public class AnnualLeaveRepository : ISerializable, IAnnualLeaveRepository
{
    private readonly string _fileName = @".\..\..\..\..\Data\annualLeave.json";
    private List<AnnualLeave>? _annualLeave;

    public AnnualLeaveRepository()
    {
        _annualLeave = new List<AnnualLeave>();
        Serializer.Load(this);
    }

    public void Insert(AnnualLeave newAnnualLeave)
    {
        _annualLeave.Add(newAnnualLeave);

        Serializer.Save(this);
    }

    string ISerializable.FileName()
    {
        return _fileName;
    }

    IEnumerable<object>? ISerializable.GetList()
    {
        return _annualLeave;
    }

    void ISerializable.Import(JToken token)
    {
        _annualLeave = token.ToObject<List<AnnualLeave>>();
    }
    public void SaveChanges()
    {
        Serializer.Save(this);
    }

    public IEnumerable<AnnualLeave> GetAll()
    {
        return _annualLeave;
    }

    public void Delete(AnnualLeave entity)
    {
        _annualLeave.Remove(entity);
        Serializer.Save(this);
    }

    public AnnualLeave GetById(int id)
    {
        return _annualLeave.FirstOrDefault(annualLeave => annualLeave.Id == id);
    }
    
    public void UpdateStatus(int id, AnnualLeave.Status status)
    {
        GetById(id).RequestStatus = status;
        Serializer.Save(this);
    }
}
