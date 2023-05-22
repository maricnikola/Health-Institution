using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.UsersRepo;

namespace ZdravoCorp.Core.Services.PatientServices;

public class PatientService : IPatientService
{
    private IPatientRepository _patientRepository;
    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }
    public List<Patient>? GetAll()
    {
        return _patientRepository.GetAll() as List<Patient>;
    }

    public Patient? GetByEmail(string email)
    {
        return _patientRepository.GetByEmail(email);
    }

    public void AddPatient(PatientDTO patient)
    {
        _patientRepository.Insert(new Patient(patient));
    }

    public void Update(string email, PatientDTO patientDto)
    {
        throw new NotImplementedException();
    }

    public void Delete(string email)
    {
        throw new NotImplementedException();
    }
}