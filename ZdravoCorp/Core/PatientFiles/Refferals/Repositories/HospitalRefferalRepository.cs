using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.PatientFiles.Refferals.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.PatientFiles.Refferals.Repositories;

public class HospitalRefferalRepository: ISerializable, IHospitalRefferalRepository
{
    private readonly string _fileName = @".\..\..\..\..\Data\hospitalRefferal.json";
    private List<HospitalRefferal>? _hospitalRefferals;

    public HospitalRefferalRepository()
    {
        _hospitalRefferals = new List<HospitalRefferal>();
        Serializer.Load(this);
    }

    public void Insert(HospitalRefferal newHospitalRefferal)
    {
        _hospitalRefferals.Add(newHospitalRefferal);

        Serializer.Save(this);
    }

    string ISerializable.FileName()
    {
        return _fileName;
    }

    IEnumerable<object>? ISerializable.GetList()
    {
        return _hospitalRefferals;
    }

    void ISerializable.Import(JToken token)
    {
        _hospitalRefferals = token.ToObject<List<HospitalRefferal>>();
    }
    public void SaveChanges()
    {
        Serializer.Save(this);
    }

    public List<HospitalRefferal> GetAll()
    {
        return _hospitalRefferals;
    }

    public void Delete(HospitalRefferal entity)
    {
        _hospitalRefferals.Remove(entity);
        Serializer.Save(this);
    }

    public HospitalRefferal GetById(int id)
    {
        return _hospitalRefferals.FirstOrDefault(refferal => refferal.Id == id);
    }
    public void UpdateControlAppointment(HospitalRefferal entity,bool status)
    {
        entity.ControlAppointment = status;
        Serializer.Save(this);
    }
}
