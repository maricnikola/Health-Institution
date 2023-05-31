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

public class DoctorSurvayRepository : ISerializable, IDoctorSurvayRepository
{
    private readonly string _fileName = @".\..\..\..\Data\doctorSurvays.json";
    private List<DoctorSurvay>? _survays;

    public DoctorSurvayRepository()
    {
        _survays = new List<DoctorSurvay>();
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
        _survays = token.ToObject<List<DoctorSurvay>>();

    }

    public IEnumerable<DoctorSurvay> GetAll()
    {
        return _survays;
    }

    public void Insert(DoctorSurvay survay)
    {
        _survays.Add(survay);
        Serializer.Save(this);
    }

    public void Delete(DoctorSurvay survay)
    {
        _survays.Remove(survay);
        Serializer.Save(this);
    }

    public DoctorSurvay GetById(int id)
    {
        return _survays.FirstOrDefault(survay=> survay.Id == id);
    }
}