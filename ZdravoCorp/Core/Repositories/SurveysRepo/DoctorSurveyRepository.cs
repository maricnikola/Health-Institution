﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Notifications;
using ZdravoCorp.Core.Models.Surveys;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.SurveysRepo;

public class DoctorSurveyRepository : ISerializable, IDoctorSurveyRepository
{
    private readonly string _fileName = @".\..\..\..\Data\doctorSurveys.json";
    private List<DoctorSurvey>? _surveys;

    public DoctorSurveyRepository()
    {
        _surveys = new List<DoctorSurvey>();
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
        _surveys = token.ToObject<List<DoctorSurvey>>();
    }

    public IEnumerable<DoctorSurvey> GetAll()
    {
        return _surveys;
    }

    public void Insert(DoctorSurvey survey)
    {
        _surveys.Add(survey);
        Serializer.Save(this);
    }

    public void Delete(DoctorSurvey survey)
    {
        _surveys.Remove(survey);
        Serializer.Save(this);
    }

    public DoctorSurvey GetById(int id)
    {
        return _surveys.FirstOrDefault(survey=> survey.Id == id);
    }
}