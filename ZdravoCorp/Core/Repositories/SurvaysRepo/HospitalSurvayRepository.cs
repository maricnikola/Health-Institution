using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Survays;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.SurvaysRepo;

public class HospitalSurvayRepository : ISerializable, IHospitalSurvayRepository
{
    private readonly string _fileName = @".\..\..\..\Data\hospitalSurvays.json";
    private List<HospitalSurvay>? _survays;

    public HospitalSurvayRepository()
    {
        _survays = new List<HospitalSurvay>();
        Serializer.Load(this);
    }
    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _survays;
    }

    public void Import(JToken token)
    {
        _survays = token.ToObject<List<HospitalSurvay>>();
    }

    public IEnumerable<HospitalSurvay> GetAll()
    {
        return _survays;
    }

    public void Insert(HospitalSurvay survay)
    {
        _survays.Add(survay);
        Serializer.Save(this);
    }

    public void Delete(HospitalSurvay survay)
    {
        _survays.Remove(survay);
        Serializer.Save(this);
    }

    public HospitalSurvay GetById(int id)
    {
        throw new NotImplementedException();
    }
}