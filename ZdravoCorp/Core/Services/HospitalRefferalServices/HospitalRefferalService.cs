using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.HospitalRefferals;
using ZdravoCorp.Core.Models.SpecialistsRefferals;
using ZdravoCorp.Core.Repositories.HospitalRefferalsRepo;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Services.HospitalRefferalServices;

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
                patientEmail.Equals(refferal.PatientMail) && !time.Overlap(refferal.Time));
    }
}
