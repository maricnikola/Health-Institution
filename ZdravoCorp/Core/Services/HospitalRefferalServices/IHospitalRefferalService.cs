using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.HospitalRefferals;

namespace ZdravoCorp.Core.Services.HospitalRefferalServices;

public interface IHospitalRefferalService
{
    public List<HospitalRefferal>? GetAll();
    public HospitalRefferal? GetById(int id);
    public void AddRefferal(HospitalRefferal hospitalRefferal);
    public void Delete(int id);
}
