using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Notifications;
using ZdravoCorp.Core.Models.Survays;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.SurvaysRepo;

public class DoctorSurveyRepository : ISerializable, IDoctorSurveyRepository
{
    private readonly string _fileName = @".\..\..\..\Data\doctorSurveys.json";
    private List<DoctorSurvey>? _survays;

    public DoctorSurveyRepository()
    {
        _survays = new List<DoctorSurvey>();
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
        _survays = token.ToObject<List<DoctorSurvey>>();
    }

    public IEnumerable<DoctorSurvey> GetAll()
    {
        return _survays;
    }

    public void Insert(DoctorSurvey survay)
    {
        _survays.Add(survay);
        Serializer.Save(this);
    }

    public void Delete(DoctorSurvey survay)
    {
        _survays.Remove(survay);
        Serializer.Save(this);
    }

    public DoctorSurvey GetById(int id)
    {
        return _survays.FirstOrDefault(survay=> survay.Id == id);
    }
}