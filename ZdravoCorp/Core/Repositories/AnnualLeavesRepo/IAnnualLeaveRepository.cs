using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.AnnualLeaves;
using ZdravoCorp.Core.Models.HospitalRefferals;

namespace ZdravoCorp.Core.Repositories.AnnualLeavesRepo;

public interface IAnnualLeaveRepository : IRepository<AnnualLeave>
{
    public void UpdateStatus(int id, AnnualLeave.Status status);

}
