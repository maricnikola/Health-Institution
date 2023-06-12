using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Survays;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.SurvaysRepo;

public class HospitalSurveyRepository : ISerializable, IHospitalSurveyRepository
{
    private readonly string _fileName = @".\..\..\..\Data\hospitalSurveys.json";
    private List<HospitalSurvey>? _survays;

    public HospitalSurveyRepository()
    {
        _survays = new List<HospitalSurvey>();
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
        _survays = token.ToObject<List<HospitalSurvey>>();
    }

    public IEnumerable<HospitalSurvey> GetAll()
    {
        return _survays;
    }

    public void Insert(HospitalSurvey survay)
    {
        _survays.Add(survay);
        Serializer.Save(this);
    }

    public void Delete(HospitalSurvey survay)
    {
        _survays.Remove(survay);
        Serializer.Save(this);
    }

    public HospitalSurvey GetById(int id)
    {
        throw new NotImplementedException();
    }
}