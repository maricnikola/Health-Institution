using System.Collections.Generic;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.Services.DoctorServices;

public interface IDoctorService
{
    public List<Doctor>? GetAll();
    public Doctor? GetByEmail(string email);
    public void AddDoctor(DoctorDTO doctor);
    public void Update(string email, DoctorDTO doctorDto);
    public void Delete(string email);
    public List<Doctor> GetAllWithCertainSpecialization(Doctor.SpecializationType specialization);
    public List<Doctor> GetAllSpecialized(Doctor.SpecializationType specializationType);
}