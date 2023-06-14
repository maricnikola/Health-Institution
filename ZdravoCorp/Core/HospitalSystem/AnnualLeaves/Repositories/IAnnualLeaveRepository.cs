using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Repositories;

public interface IAnnualLeaveRepository : IRepository<AnnualLeave>
{
    public void UpdateStatus(int id, AnnualLeave.Status status);

}
