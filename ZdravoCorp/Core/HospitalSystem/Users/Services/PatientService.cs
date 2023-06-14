using System;
using System.Collections.Generic;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Repositories;

namespace ZdravoCorp.Core.HospitalSystem.Users.Services;

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

    public void ChangeTimeForNotification(string email, TimeSpan newTimeSpan)
    {
        _patientRepository.UpdateNotificationTime(email,newTimeSpan);
    }
}