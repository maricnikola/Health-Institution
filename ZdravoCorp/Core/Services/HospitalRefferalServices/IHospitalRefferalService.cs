using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Appointments;
using ZdravoCorp.Core.Models.HospitalRefferals;
using ZdravoCorp.Core.Models.Therapies;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Services.HospitalRefferalServices;

public interface IHospitalRefferalService
{
    public List<HospitalRefferal>? GetAll();
    public HospitalRefferal? GetById(int id);
    public void AddRefferal(HospitalRefferal hospitalRefferal);
    public void Delete(int id);
    public bool IsPatientOnHospitalTreatment(string patientEmail,TimeSlot time);
    public void AddNewTherapy(Therapy therapy, int id);

}
