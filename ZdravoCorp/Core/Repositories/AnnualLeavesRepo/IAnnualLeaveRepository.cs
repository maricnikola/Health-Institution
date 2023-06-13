using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.AnnualLeaves;
using ZdravoCorp.Core.Models.HospitalRefferals;

namespace ZdravoCorp.Core.Repositories.AnnualLeavesRepo;

public interface IAnnualLeaveRepository
{
    public void Insert(AnnualLeave newAnnualLeave);
    public void SaveChanges();
    public List<AnnualLeave> GetAll();
    public void Delete(AnnualLeave entity);
    public AnnualLeave GetById(int id);
}
