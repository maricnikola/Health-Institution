using System.Collections.Generic;
using ZdravoCorp.Core.PatientFiles.Refferals.Models;

namespace ZdravoCorp.Core.PatientFiles.Refferals.Repositories;

public interface IHospitalRefferalRepository
{
    public void Insert(HospitalRefferal newHospitalRefferal);
    public void SaveChanges();
    public List<HospitalRefferal> GetAll();
    public void Delete(HospitalRefferal entity);
    public HospitalRefferal GetById(int id);
    public void UpdateControlAppointment(HospitalRefferal entity,bool status);

}
