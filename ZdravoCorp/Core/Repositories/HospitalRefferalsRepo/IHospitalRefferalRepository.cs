using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.HospitalRefferals;

namespace ZdravoCorp.Core.Repositories.HospitalRefferalsRepo;

public interface IHospitalRefferalRepository
{
    public void Insert(HospitalRefferal newHospitalRefferal);
    public void SaveChanges();
    public List<HospitalRefferal> GetAll();
    public void Delete(HospitalRefferal entity);
    public HospitalRefferal GetById(int id);
}
