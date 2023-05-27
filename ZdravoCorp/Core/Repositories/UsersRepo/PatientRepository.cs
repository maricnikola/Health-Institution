using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.UsersRepo;


public class PatientRepository : ISerializable, IPatientRepository
{
    private readonly string _fileName = @".\..\..\..\Data\patients.json";


    public PatientRepository()
    {
        Patients = new List<Patient>();
        Serializer.Load(this);
    }

    public List<Patient> Patients { get; private set; }


    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return Patients;
    }

    public void Import(JToken token)
    {
        Patients = token.ToObject<List<Patient>>();
    }

    public void Add(Patient patient)
    {
        Patients.Add(patient);
    }
    
    public IEnumerable<Patient> GetAll()
    {
        return Patients;
    }

    public void Insert(Patient entity)
    {
        Patients.Add(entity);
    }

    public Patient? GetByEmail(string email)
    {
        return Patients.FirstOrDefault(patient => patient.Email == email);
    }

    public void UpdateNotificationTime(string email, TimeSpan newTimeSpan)
    {
        GetByEmail(email)!.NotificationTime=newTimeSpan;
        Serializer.Save(this);
    }
}