using System;
using System.Collections.Generic;
using ZdravoCorp.Core.HospitalSystem.Users.Models;

namespace ZdravoCorp.Core.HospitalSystem.Users.Services;

public interface IPatientService
{
    public List<Patient>? GetAll();
    public Patient? GetByEmail(string email);
    public void AddPatient(PatientDTO patient);
    public void Update(string email, PatientDTO patientDto);
    public void Delete(string email);
    public void ChangeTimeForNotification(string email, TimeSpan newTimeSpan);
}