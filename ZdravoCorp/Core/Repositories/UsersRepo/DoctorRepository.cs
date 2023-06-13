using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.UsersRepo;



public class DoctorRepository : ISerializable, IDoctorRepository
{
    private readonly string _fileName = @".\..\..\..\..\Data\doctors.json";


    public DoctorRepository()
    {
        _doctors = new List<Doctor>();
        Serializer.Load(this);
    }

    private List<Doctor>? _doctors;

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


    IEnumerable<Doctor> IUserRepository<Doctor>.GetAll()
    {
        return GetAll();
    }

    public void Insert(Doctor entity)
    {
        _doctors.Add(entity);
    }

    public Doctor? GetByEmail(string email)
    {
        return _doctors.FirstOrDefault(doctor => doctor.Email == email);
    }

    public void UpdateGrade(string email, double grade)
    {
        GetByEmail(email).Grade = grade;
        Serializer.Save(this);
    }

    public List<Doctor> GetAll()
    {
        return _doctors;
    }


}