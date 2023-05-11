using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Documents;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Exceptions;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.User;

public class DoctorRepository : ISerializable
{

    private  List<Doctor>? _doctors;
    private readonly string _fileName = @".\..\..\..\Data\doctors.json";
    public List<Doctor>? Doctors => _doctors;
    
    

    public DoctorRepository()
    {
        _doctors = new List<Doctor>();
        Serializer.Load(this);
    }


    public Doctor? GetDoctorByEmail(string email)
    {
        return _doctors.FirstOrDefault(doctor => doctor.Email == email);
    }

    public List<Doctor> GetAll()
    {
        return _doctors;
    }

    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
       return _doctors;
    }

    public void Import(JToken token)
    {
        _doctors = token.ToObject<List<Doctor>>();
    }

    public List<Doctor> GetAllSpecialized(Doctor.SpecializationType specializationType)
    {
        List<Doctor> suitableDoctors = Doctors.FindAll(doctor => doctor.Specialization == specializationType);
        return suitableDoctors;
    
    }

}
