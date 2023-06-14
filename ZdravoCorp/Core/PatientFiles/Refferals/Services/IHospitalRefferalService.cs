using System;
using System.Collections.Generic;
using ZdravoCorp.Core.PatientFiles.Refferals.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.PatientFiles.Refferals.Services;

public interface IHospitalRefferalService
{
    public List<HospitalRefferal>? GetAll();
    public HospitalRefferal? GetById(int id);
    public void AddRefferal(HospitalRefferal hospitalRefferal);
    public void Delete(int id);
    public bool IsPatientOnHospitalTreatment(string patientEmail,TimeSlot time);
    public void AddNewTherapy(Therapy therapy, int id);
    public bool CheckNewEndDate(DateTime endDate, int id);

    public bool UpdateEndDate(int id, DateTime endDate);
    public void UpdateControlAppointment(int id,bool status);



}
