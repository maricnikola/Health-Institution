using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.UsersRepo;

namespace ZdravoCorp.Core.Services.DoctorServices;

public class DoctorService : IDoctorService
{
    private IDoctorRepository _doctorRepository;

    public DoctorService(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public List<Doctor>? GetAll()
    {
        return _doctorRepository.GetAll() as List<Doctor>;
    }

    public Doctor? GetByEmail(string email)
    {
        return _doctorRepository.GetByEmail(email);
    }

    public void AddDoctor(DoctorDTO doctor)
    {
        _doctorRepository.Insert(new Doctor(doctor));
    }

    public void Update(string email, DoctorDTO doctorDto)
    {
        throw new NotImplementedException();
    }

    public void Delete(string email)
    {
        throw new NotImplementedException();
    }

    public List<Doctor> GetAllWithCertainSpecialization(Doctor.SpecializationType specialization)
    {
        return _doctorRepository.GetAll().Where(doctor => doctor.Specialization == specialization).ToList();
    }

    public List<Doctor> GetAllSpecialized(Doctor.SpecializationType specializationType)
    {
        var suitableDoctors = _doctorRepository.GetAll().Where(doctor => doctor.Specialization == specializationType);
        return suitableDoctors.ToList();
    }

    public void UpdateGrade(string email, double grade)
    {
        _doctorRepository.UpdateGrade(email, grade);
    }
}