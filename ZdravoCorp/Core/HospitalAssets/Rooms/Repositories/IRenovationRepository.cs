using ZdravoCorp.Core.HospitalAssets.Rooms.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalAssets.Rooms.Repositories;

public interface IRenovationRepository: IRepository<Renovation>
{
    void UpdateStatus(int id, Renovation.RenovationStatus status);
}