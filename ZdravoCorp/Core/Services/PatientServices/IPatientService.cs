using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Users;

namespace ZdravoCorp.Core.Services.PatientServices;

public interface IPatientService
{
    public List<Patient>? GetAll();
    public Patient? GetByEmail(string email);
    public void AddPatient(PatientDTO patient);
    public void Update(string email, PatientDTO patientDto);
    public void Delete(string email);
    public void ChangeTimeForNotification(string email, TimeSpan newTimeSpan);
}