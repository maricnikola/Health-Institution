using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.UsersRepo;



public class DoctorRepository : ISerializable, IDoctorRepository
{
    private readonly string _fileName = @".\..\..\..\Data\doctors.json";


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


    public Doctor? GetDoctorByEmail(string email)
    {
        return _doctors.FirstOrDefault(doctor => doctor.Email == email);
    }

    public List<Doctor> GetAll()
    {
        return _doctors;
    }

    public List<Doctor> GetAllWithCertainSpecialization(Doctor.SpecializationType specialization)
    {
        var wantedDoctors = new List<Doctor>();
        foreach (var doctor in _doctors)
            if (doctor.Specialization == specialization)
                wantedDoctors.Add(doctor);
        return wantedDoctors;
    }

    public List<Doctor> GetAllSpecialized(Doctor.SpecializationType specializationType)
    {
        var suitableDoctors = _doctors.FindAll(doctor => doctor.Specialization == specializationType);
        return suitableDoctors;
    }
}