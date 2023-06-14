using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.HospitalSystem.Analytics.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalSystem.Analytics.Repositories;

public class HospitalSurveyRepository : ISerializable, IHospitalSurveyRepository
{
    private readonly string _fileName = @".\..\..\..\..\Data\hospitalSurveys.json";
    private List<HospitalSurvey>? _surveys;

    public HospitalSurveyRepository()
    {
        _surveys = new List<HospitalSurvey>();
        Serializer.Load(this);
    }
    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _surveys;
    }

    public void Import(JToken token)
    {
        _surveys = token.ToObject<List<HospitalSurvey>>();
    }

    public IEnumerable<HospitalSurvey> GetAll()
    {
        return _surveys;
    }

    public void Insert(HospitalSurvey survey)
    {
        _surveys.Add(survey);
        Serializer.Save(this);
    }

    public void Delete(HospitalSurvey survey)
    {
        _surveys.Remove(survey);
        Serializer.Save(this);
    }

    public HospitalSurvey GetById(int id)
    {
        throw new NotImplementedException();
    }
}