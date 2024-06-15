using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Core.PatientFiles.Refferals.Models;
using ZdravoCorp.Core.PatientFiles.Refferals.Repositories;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.PatientFiles.Refferals.Services;

public class HospitalRefferalService : IHospitalRefferalService
{
    private IHospitalRefferalRepository _hospitalRefferalRepository;

    public HospitalRefferalService(IHospitalRefferalRepository hospitalRefferalRepository)
    {
        _hospitalRefferalRepository = hospitalRefferalRepository;
    }

    public List<HospitalRefferal>? GetAll()
    {
        return _hospitalRefferalRepository.GetAll() as List<HospitalRefferal>;
    }

    public HospitalRefferal? GetById(int id)
    {
        return _hospitalRefferalRepository.GetById(id);
    }

    public void AddRefferal(HospitalRefferal specialistsRefferal)
    {
        _hospitalRefferalRepository.Insert(specialistsRefferal);
    }

    public void Delete(int id)
    {
        _hospitalRefferalRepository.Delete(GetById(id));
    }
    public bool IsPatientOnHospitalTreatment(string patientEmail,TimeSlot time)
    {
        return _hospitalRefferalRepository.GetAll().Any(refferal =>
                patientEmail.Equals(refferal.PatientMail) && time.Overlap(refferal.Time));
    }
    public void AddNewTherapy(Therapy therapy,int id)
    {
        var hospitalRefferal = GetById(id);
        foreach(Therapy therapInvalid in hospitalRefferal.InitialTherapy)
        {
            therapInvalid.Status = false;
        }
        hospitalRefferal.InitialTherapy.Add(therapy);
        _hospitalRefferalRepository.SaveChanges();

    }
    public bool CheckNewEndDate(DateTime endDate,int id)
    {
        var hospitalRefferal = GetById(id);
        var now = DateTime.Now;
        if (now.Date > endDate.Date || hospitalRefferal.Time.Start.Date > endDate.Date) return false;
        return true;
    }
    public bool UpdateEndDate(int id,DateTime endDate)
    {
        var hospitalRefferal = GetById(id);
        if (!CheckNewEndDate(endDate, id)) return false;
        hospitalRefferal.Time.End = endDate;
        _hospitalRefferalRepository.SaveChanges();
        return true;
    }
    public void UpdateControlAppointment(int id,bool status)
    {
        var hospitalRefferal = GetById(id);
        _hospitalRefferalRepository.UpdateControlAppointment(hospitalRefferal,status);
    }
}
